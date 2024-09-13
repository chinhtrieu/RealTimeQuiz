using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Services
{
    public interface IRedisService
    {
        Task<bool> IsMemberOfSetAsync(string setKey, string value);
        Task AddToSetAsync(string setKey, string value);
        Task<double> UpdateSortedSetMemberScore(string key, string sortedSetMember, int score);
        Task<RedisValue[]> ReadFromSortedSetByRank(string sortedSetKey, long start, long stop);

    }
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public async Task<bool> IsMemberOfSetAsync(string setKey, string value)
        {
            return await _db.SetContainsAsync(setKey, value);
        }
        public async Task AddToSetAsync(string setKey, string value)
        {
            await _db.SetAddAsync(setKey, value);
        }
        public async Task<RedisValue[]> ReadFromSortedSetByRank(string sortedSetKey, long start, long stop)
        {
            return await _db.SortedSetRangeByRankAsync(sortedSetKey, start, stop);

        }

        public async Task<double> UpdateSortedSetMemberScore(string key, string member, int score)
        {
            return await _db.SortedSetIncrementAsync(key, member, Convert.ToDouble(score));
        }

    }
}

