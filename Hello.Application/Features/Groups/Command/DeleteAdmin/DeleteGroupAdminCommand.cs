using Hello.Application.Features.Groups.Command.Delete;
using Hello.Application.Hubs;
using Hello.Domain.Dtos;
using Hello.Domain.Interfaces;
using Hello.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Application.Features.Groups.Command.DeleteAdmin
{
    public class DeleteGroupAdminCommand:IRequest<Result<bool>>
    {
        public string UserId { get; set; }
        public int GroupId { get; set; }

    }

    internal class DeleteGroupAdminCommandHandler : IRequestHandler<DeleteGroupAdminCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub,IChatHub> _context;
        private readonly UserManager<User> _userManager;

        public DeleteGroupAdminCommandHandler(
            IUnitOfWork unitOfWork,
            IHubContext<ChatHub, IChatHub> context,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(DeleteGroupAdminCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.GroupMemberRepo.Entities
                .Where(x => x.MemberId == request.UserId && x.GroupId == request.GroupId)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsAdmin, false), cancellationToken);

            var user = await _userManager.Users.Where(x => x.Id == request.UserId)
                .Select(x => x.FirstName + ' ' + x.LastName)
                .FirstOrDefaultAsync();

            if(user == null)
            {
                return Result<bool>.Failure("User not found.", HttpStatusCode.NotFound);
            }

            await _context.Clients.Group(request.GroupId.ToString()).ReceiveGroupUpdate($"{user} removed from group admin.");

            return Result<bool>.Success(true);
        }
    }
}
