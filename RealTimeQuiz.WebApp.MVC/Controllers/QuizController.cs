using Microsoft.AspNetCore.Mvc;

namespace RealTimeQuiz.WebApp.MVC.Controllers
{
    public class QuizController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public QuizController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var userId = Request.Query["UserId"];
            ViewBag.UserId = userId;
            ViewBag.QuizId = "quiz";
            return View();
        }    
    }
}
