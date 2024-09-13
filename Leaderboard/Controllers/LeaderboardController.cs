using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeQuiz.Services;
using RealTimeQuiz.Shared.Models;

namespace Leaderboard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {       
        private readonly IRedisService _redisService;
        private readonly ILogger<LeaderboardController> _logger;
        public LeaderboardController(IRedisService redisService, ILogger<LeaderboardController> logger)
        {
            _redisService = redisService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Get ReadFromSortedSetByRank");
            var cacheValues = await _redisService.ReadFromSortedSetByRank("leaderboard", 0, 10);

            LeaderboardModel leaderboardModel = new LeaderboardModel
            {
                Members = new List<LeaderboardMember>()
                
            };
            foreach (var item in cacheValues)
            {
                leaderboardModel.Members.Add(new LeaderboardMember { UserId = item.ToString() });
            }
            
            return Ok(leaderboardModel);
        }
    }
}
