using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeQuiz.Services;
using RealTimeQuiz.Shared.Models;

namespace ScoreUpdates.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly IRedisService _redisService;
        public ScoresController(IRedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateScore(UpdateScoreRequest request)
        {
            var newScore = await _redisService.UpdateSortedSetMemberScore("leaderboard", request.UserId, request.Score);
            return Ok(new UpdateScoreResult { Success = true, NewScore = newScore });    

        }
    }
}
