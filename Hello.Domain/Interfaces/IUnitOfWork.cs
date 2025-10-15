using Hello.Domain.Interfaces.Repo;
using Hello.Domain.Models;
using StackExchange.Redis;

namespace Hello.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        IBaseRepository<Chat> ChatRepo { get; }
        IBaseRepository<Group> GroupRepo { get; }
        IBaseRepository<GroupChat> GroupChatRepo { get; }
        IBaseRepository<GroupMember> GroupMemberRepo { get; }
        IBaseRepository<Community> CommunityRepo { get; }
        IBaseRepository<CommunityChat> CommunityChatRepo { get; }
        IBaseRepository<CommunityMember> CommunityMemberRepo { get; }
        IBaseRepository<GroupChatRead> GroupChatReadRepo { get; }
        IDatabase Redis { get; }
        IUserNameBloomServices UserNameBloomServices { get; }
    }
}
