using FluentValidation;
using Hello.Application.Hubs;
using Hello.Domain;
using Hello.Domain.Dtos;
using Hello.Domain.Interfaces;
using Hello.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hello.Application.Features.Groups.Command.Create
{
    public class CreateGroupCommand : IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageBase64 { get; set; }
        public List<string> MemberIds { get; set; }
    }

    internal class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<CreateGroupCommand> _validator;
        private readonly IHubContext<ChatHub, IChatHub> _hubContext;

        public CreateGroupCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context,
            IValidator<CreateGroupCommand> validator,
            IHubContext<ChatHub, IChatHub> hubContext,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _validator = validator;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        public async Task<Result<int>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Result<int>.ValidationFailure(validationResult.Errors);
            }

            var userId = _context.HttpContext!.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value!;

            var user = await _userManager.Users
                .Where(x => x.Id == userId)
                .Select(x => x.FirstName + ' ' + x.LastName)
                .FirstAsync(cancellationToken);

            var group = request.Adapt<Group>();
            group.CreatedBy = userId;

            await _unitOfWork.GroupRepo.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            var adminMember = new GroupMember
            {
                GroupId = group.Id,
                MemberId = userId,
                IsAdmin = true
            };

            var members = request.MemberIds.Distinct().Select(id => new GroupMember
            {
                GroupId = group.Id,
                MemberId = id
            }).ToList();

            members.Add(adminMember);

            await _unitOfWork.GroupMemberRepo.AddRange(members);
            await _unitOfWork.SaveChangesAsync();

            var userConIds = new List<string>();

            foreach (var member in members)
            {
                var memberConIds = await _unitOfWork.Redis.SetMembersAsync(Constants.Redis_UserId_Prefix + member.MemberId);

                if(member.MemberId == userId)
                {
                    userConIds.AddRange(memberConIds.Select(x => x.ToString()));
                }

                foreach (var conId in memberConIds)
                {
                    await _hubContext.Groups.AddToGroupAsync(conId!, group.Id.ToString(), cancellationToken);
                }
            }

            await _hubContext.Clients.GroupExcept(group.Id.ToString(), userConIds).ReceiveGroupUpdate($"{user} added you to {group.Name}");

            return Result<int>.Success(data: group.Id);
        }
    }
}
