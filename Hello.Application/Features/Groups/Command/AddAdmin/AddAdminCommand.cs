using Hello.Application.Hubs;
using Hello.Domain.Interfaces;
using Hello.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hello.Application.Features.Groups.Command.AddAdmin
{
    public class AddAdminCommand : IRequest
    {
        public int GroupId { get; set; }
        public string MemberId { get; set; }
    }

    internal class AddAdminCommandHandler : IRequestHandler<AddAdminCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub, IChatHub> _hubContext;
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<User> _userManager;

        public AddAdminCommandHandler(
            IUnitOfWork unitOfWork,
            IHubContext<ChatHub, IChatHub> hubContext,
            IHttpContextAccessor context,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _context = context;
            _userManager = userManager;
        }

        public async Task Handle(AddAdminCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_context.HttpContext.User);

            var userToAdd = await _userManager.FindByIdAsync(request.MemberId);

            await _unitOfWork.GroupMemberRepo.Entities
                .Where(x => x.MemberId == request.MemberId && x.GroupId == request.GroupId)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsAdmin, true), cancellationToken);

            await _hubContext.Clients.Group(request.GroupId.ToString())
                .ReceiveGroupUpdate($"{user.FirstName} {user.LastName} set {userToAdd.FirstName} {user.LastName} admin");
        }
    }
}
