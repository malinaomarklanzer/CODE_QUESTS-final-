using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CODE_QUESTS.Controllers
{
    // This [Authorize] attribute locks the *ENTIRE* controller.
    // Only users in the "Admin" role can access any page in here.
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // This will be the homepage for your admin panel
        public IActionResult Index()
        {
            // Later, you will add code here to get all questions from the database
            return View();
        }

        // Add other methods here later, like:
        // public IActionResult CreateQuestion() { ... }
        // public IActionResult EditQuestion(int id) { ... }
    }
}