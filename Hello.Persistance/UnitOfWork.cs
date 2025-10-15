using Hello.Domain.Interfaces;
using Hello.Domain.Interfaces.Repo;
using Hello.Domain.Models;
using StackExchange.Redis;

namespace Hello.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HelloDbContext _context;
        private readonly IDatabase _redis;

        public UnitOfWork(
            HelloDbContext context,
            IConnectionMultiplexer redis,
            IBaseRepository<Chat> chatRepo,
            IBaseRepository<Group> groupRepo,
            IBaseRepository<GroupChat> groupChatRepo,
            IBaseRepository<GroupMember> groupMemberRepo,
            IBaseRepository<Community> communityRepo,
            IBaseRepository<CommunityChat> communityChatRepo,
            IBaseRepository<CommunityMember> communityMemberRepo,
            IUserNameBloomServices userNameBloomServices,
            IBaseRepository<GroupChatRead> groupChatReadRepo)
        {
            _context = context;
            _redis = redis.GetDatabase();
            ChatRepo = chatRepo;
            GroupRepo = groupRepo;
            GroupChatRepo = groupChatRepo;
            GroupMemberRepo = groupMemberRepo;
            CommunityRepo = communityRepo;
            CommunityChatRepo = communityChatRepo;
            CommunityMemberRepo = communityMemberRepo;
            UserNameBloomServices = userNameBloomServices;
            GroupChatReadRepo = groupChatReadRepo;
        }

        public IBaseRepository<Chat> ChatRepo { get; private set; }

        public IBaseRepository<Group> GroupRepo { get; private set; }

        public IBaseRepository<GroupChat> GroupChatRepo { get; private set; }

        public IBaseRepository<GroupMember> GroupMemberRepo { get; private set; }

        public IBaseRepository<Community> CommunityRepo { get; private set; }

        public IBaseRepository<CommunityChat> CommunityChatRepo { get; private set; }

        public IBaseRepository<CommunityMember> CommunityMemberRepo { get; private set; }

        public IDatabase Redis => _redis;

        public IUserNameBloomServices UserNameBloomServices { get; private set; }

        public IBaseRepository<GroupChatRead> GroupChatReadRepo { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
