using Hello.Application.Dtos.Reqeust;
using Hello.Application.Dtos.Response;
using Hello.Domain;
using Hello.Domain.Dtos.Reqeust;
using Hello.Domain.Dtos.Response;
using Hello.Domain.Interfaces;
using Hello.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hello.Application.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatHub>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMediaServices _mediaServices;

        public ChatHub(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMediaServices mediaServices)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mediaServices = mediaServices;
        }

        public override async Task OnConnectedAsync()
        {
            var userGroups = await _unitOfWork.GroupMemberRepo.Entities
                .Where(x => x.MemberId == Context.UserIdentifier)
                .Select(x => x.GroupId)
                .ToListAsync();

            if (userGroups.Count > 0)
            {
                userGroups.ForEach(async groupId =>
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
                });
            }

            await _unitOfWork.Redis.StringSetAsync(Constants.Redis_Connection_Prefix + Context.ConnectionId, Context.UserIdentifier);
            await _unitOfWork.Redis.SetAddAsync(Constants.Redis_UserId_Prefix + Context.UserIdentifier, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(SendPrivateMessageDto dto)
        {
            var receiverConnectionIds = await _unitOfWork.Redis.SetMembersAsync(Constants.Redis_UserId_Prefix + dto.UserId);

            var user = await _userManager.Users
                .Where(x => x.Id == Context.UserIdentifier)
                .Select(x => x.FirstName + ' ' + x.LastName)
                .FirstAsync();

            var imageToByte = dto.ImageBase64 != null ? Convert.FromBase64String(dto.ImageBase64) : null;

            var uploadResult = dto.ImageBase64 != null ? await _mediaServices.UploadMediaAsync(imageToByte) : null;

            var message = new Chat()
            {
                ReceiverId = dto.UserId,
                SenderId = Context.UserIdentifier!,
                ImageUrl = uploadResult?.Url,
                ImageId = uploadResult?.FileId,
                Message = dto.Message
            };

            await _unitOfWork.ChatRepo.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            foreach (var conId in receiverConnectionIds)
            {
                await Clients.Client(conId).ReceivePrivateMessage(new ReceivePrivateMessageDto()
                {
                    Name = user,
                    ImageUrl = uploadResult.Url,
                    Message = dto.Message,
                    MessageId = message.Id
                });
            }
        }

        public async Task JoinGroup(int groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());

            var member = new GroupMember()
            {
                GroupId = groupId,
                MemberId = Context.UserIdentifier!
            };

            await _unitOfWork.GroupMemberRepo.AddAsync(member);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendGroupMessage(SendGroupMessageDto dto)
        {
            var userId = Context.UserIdentifier;

            var user = await _userManager.Users.Where(x => x.Id == userId).Select(x => new
            {
                x.Id,
                name = x.FirstName + ' ' + x.LastName
            }).FirstOrDefaultAsync();

            if (user != null)
            {
                var imageToByte = dto.ImageBase64 != null ? Convert.FromBase64String(dto.ImageBase64) : null;

                var uploadResult = dto.ImageBase64 != null ? await _mediaServices.UploadMediaAsync(imageToByte) : null;

                var groupMessage = new GroupChat()
                {
                    GroupId = dto.GroupId,
                    ImageUrl = uploadResult?.Url,
                    ImageId = uploadResult?.FileId,
                    Message = dto.Message,
                    SenderId = userId!
                };

                await _unitOfWork.GroupChatRepo.AddAsync(groupMessage);
                await _unitOfWork.SaveChangesAsync();

                var toBroadcast = new ReceiveGroupMessageDto()
                {
                    ImageUrl = uploadResult?.Url,
                    Message = dto.Message,
                    Name = user.name,
                    UserId = user.Id,
                    MessageId = groupMessage.Id
                };

                await Clients.OthersInGroup(dto.GroupId.ToString()).ReceiveGroupMessage(toBroadcast);

            }
        }

        public async Task LeaveGroup(int groupId)
        {
            var user = await _userManager.Users
                        .Where(x => x.Id == Context.UserIdentifier)
                        .Select(x => x.FirstName + ' ' + x.LastName)
                        .FirstAsync();

            await Clients.OthersInGroup(groupId.ToString()).ReceiveRemoveFromGroup($"{user} left");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());

            await _unitOfWork.GroupMemberRepo.Entities
                .Where(x => x.GroupId == groupId && x.MemberId == Context.UserIdentifier)
                .ExecuteDeleteAsync();

            if (await _unitOfWork.GroupMemberRepo.Entities.AnyAsync(x => x.GroupId == groupId))
            {
                await _unitOfWork.GroupRepo.Entities
                    .Where(x => x.Id == groupId)
                    .ExecuteDeleteAsync();
            }
        }

        public async Task ClientTyping()
        {
            var client = await _userManager.Users
                .Where(x => x.Id == Context.UserIdentifier)
                .Select(x => x.FirstName + ' ' + x.LastName)
                .FirstAsync();

            await Clients.Client(Context.UserIdentifier!).UserTyping($"{client} typing...");
        }

        public async Task StartTypingInGroup(int groupId)
        {
            var client = await _userManager.Users
                .Where(x => x.Id == Context.UserIdentifier)
                .Select(x => x.FirstName + ' ' + x.LastName)
                .FirstAsync();

            await Clients.OthersInGroup(groupId.ToString()).UserTyping($"{client} typing...");
        }

        public async Task MarkAsRead(int messageId,string userId)
        {
            await Clients.Client(userId).MarkMessageAsRead(messageId);

            await _unitOfWork.ChatRepo.Entities
                .Where(x => x.Id == messageId)
                .ExecuteUpdateAsync(x => x.SetProperty(s => s.ReadAt, DateTime.UtcNow));
        }
        
        public async Task MarkAsReadGroup(int messageId,int groupId)
        {
            await Clients.Group(groupId.ToString()).MarkMessageAsRead(messageId);

            await _unitOfWork.GroupChatReadRepo.Entities
                .Where(x => x.GroupChatId == messageId)
                .ExecuteUpdateAsync(x => x.SetProperty(s => s.ReadAt, DateTime.UtcNow));
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _unitOfWork.Redis.KeyDeleteAsync(Constants.Redis_Connection_Prefix + Context.ConnectionId);
            await _unitOfWork.Redis.SetRemoveAsync(Constants.Redis_UserId_Prefix + Context.UserIdentifier, Context.ConnectionId);

            var connections = await _unitOfWork.Redis.SetMembersAsync(Constants.Redis_UserId_Prefix + Context.UserIdentifier);

            if (connections is null || connections.Length == 0)
            {
                await _userManager.Users
                    .Where(x => x.Id == Context.UserIdentifier)
                    .ExecuteUpdateAsync(x => x.SetProperty(u => u.LastSeen, u => DateTime.UtcNow));

                await _unitOfWork.Redis.KeyDeleteAsync(Constants.Redis_UserId_Prefix + Context.UserIdentifier);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
