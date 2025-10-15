using Hello.Domain.Dtos;
using Hello.Domain.Interfaces;
using Hello.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hello.Application.Features.Chat.Queries.GetUnReadMessage
{
    public class GetUnReadMessageQuery : IRequest<Result<GetUnReadMessageQueryDto>>;

    internal class GetUnReadMessageQueryHandler : IRequestHandler<GetUnReadMessageQuery, Result<GetUnReadMessageQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public GetUnReadMessageQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Result<GetUnReadMessageQueryDto>> Handle(GetUnReadMessageQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var lastChat = await _unitOfWork.ChatRepo.Entities
                .Where(x => x.ReceiverId == userId && x.ReadAt == null)
                .GroupBy(x => new { x.SenderId, Name = x.Sender.FirstName + ' ' + x.Sender.LastName })
                .Select(x => new
                {
                    Id = x.Key.SenderId,
                    Sender = x.Key.Name,
                    Message = x.OrderByDescending(o => o.CreatedAt).Select(s => new
                    {
                        s.Message,
                        s.ImageUrl,
                        s.CreatedAt,
                    }).First(),
                    UnReadCount = x.Count()
                }).Select(x => new LastChatDto()
                {
                    Id = x.Id,
                    Sender = x.Sender,
                    Message = x.Message.Message,
                    ImageUrl = x.Message.ImageUrl,
                    CreatedAt = x.Message.CreatedAt,
                    TotalUnReadCount = x.UnReadCount
                }).ToListAsync(cancellationToken);

            var groupChat = await _unitOfWork.GroupChatRepo.Entities
                .Where(x => _unitOfWork.GroupMemberRepo.Entities.Any(g => g.MemberId == userId))
                .Where(x => !_unitOfWork.GroupChatReadRepo.Entities.Any(g => g.GroupChatId == x.Id && g.MemberId == userId))
                .GroupBy(x => new { x.GroupId, x.Group.Name })
                .Select(x => new
                {
                    x.Key.GroupId,
                    x.Key.Name,
                    Message = x.OrderByDescending(m => m.CreatedAt).Select(s => new
                    {
                        SenderName = s.Sender.FirstName + ' ' + s.Sender.LastName,
                        s.Message,
                        s.ImageUrl,
                        s.CreatedAt
                    }).First(),
                    TotalUnReadCount = x.Count()
                }).Select(x => new LastGroupChatDto()
                {
                    Id = x.GroupId,
                    Sender = x.Message.SenderName,
                    Message = x.Message.Message,
                    ImageUrl = x.Message.ImageUrl,
                    CreatedAt = x.Message.CreatedAt,
                    TotalUnreadCount = x.TotalUnReadCount
                }).ToListAsync(cancellationToken);

            return Result<GetUnReadMessageQueryDto>.Success(new GetUnReadMessageQueryDto()
            {
                Chats = lastChat,
                GroupsChats = groupChat
            });
        }
    }
}
