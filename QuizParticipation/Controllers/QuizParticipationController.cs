using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RealTimeQuiz.Services;
using RealTimeQuiz.Shared.Models;

namespace QuizParticipation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizParticipationController : ControllerBase
    {
        private readonly IRedisService _redisService;
        private readonly ILogger<QuizParticipationController> _logger;
        public QuizParticipationController(IRedisService redisService, ILogger<QuizParticipationController> logger)
        {
            _redisService = redisService;
            _logger = logger;
        }
      
        [HttpPost]
        public async Task<IActionResult> JoinQuiz(JoinQuizRequest request)
        {
            _logger.LogInformation($"User join quiz");
            if(await _redisService.IsMemberOfSetAsync("QuizParticipations", request.UserId))
            {

                return Ok(new { Success = false, Message = "User Id is existed." });
            }
            else
            {
                await _redisService.AddToSetAsync("QuizParticipations", request.UserId);
                return Ok(new { Success = true, Message = "" });

            }
        }
       
    }
}
