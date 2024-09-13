using Microsoft.AspNetCore.Mvc;
using RealTimeQuiz.Shared;
using RealTimeQuiz.Shared.Models;
using RealTimeQuiz.WebApp.MVC.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using static Prometheus.MetricServerMiddleware;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RealTimeQuiz.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _httpClientFactory = factory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string uniqueId)
        {
            _logger.LogInformation($"User {uniqueId} try to join");
            if (string.IsNullOrWhiteSpace(uniqueId))
            {
                ViewBag.Message = "User name canot be empty";
                return View();
            }
            if (uniqueId.Length > 15)
            {
                ViewBag.Message = "User name invalid";
                return View();
            }


            var client = _httpClientFactory.CreateClient(Constant.QuizParticipation);
            
            var response = await client.PostAsJsonAsync($"QuizParticipation", new JoinQuizRequest { QuizId = "QuizParticipation", UserId = uniqueId });
           
            var result = await response.Content.ReadFromJsonAsync<JoinQuizResultModel>();

            if (result.Success)
            {
                _logger.LogInformation($"User {uniqueId} joined the quiz");
                return RedirectToAction("Index", "Quiz", new { UserId = uniqueId });
            }
            else
            {

                ViewBag.Message = result.Message;
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
