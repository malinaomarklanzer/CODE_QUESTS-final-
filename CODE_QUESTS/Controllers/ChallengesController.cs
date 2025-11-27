using Microsoft.AspNetCore.Mvc;

namespace CodeQuest.Controllers
{
    public class ChallengesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}