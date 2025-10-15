using Hello.Domain.Interfaces;
using StackExchange.Redis;

namespace Hello.Persistence
{
    public class UserNameBloomServices : IUserNameBloomServices
    {
        private readonly IDatabase _database;
        private const string BloomKey = "usernames";

        public UserNameBloomServices(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task EnsureCreatedAsync()
        {
            var exists = await _database.KeyExistsAsync(BloomKey);

            var limit=1_000_000;
            var expansion = 2;
            var errorRate = 0.01;

            if (!exists)
            {
                await _database.ExecuteAsync("BF.RESERVE", BloomKey, errorRate, limit, "EXPANSION", expansion);
            }
        }

        public async Task AddUserNameAsync(string userName)
        {
            await _database.ExecuteAsync("BF.ADD", BloomKey, userName.ToLowerInvariant());
        }

        public async Task<bool> MightContainUserNameAsync(string userName)
        {
            var result = await _database.ExecuteAsync("BF.EXISTS", BloomKey, userName.ToLowerInvariant());

            return (long)result == 1;
        }
    }
}
