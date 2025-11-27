using Microsoft.AspNetCore.Authorization; // Add this line
using Microsoft.AspNetCore.Mvc;
using CODE_QUESTS.Models; // Or your project's Models namespace
using System.Diagnostics;

namespace CODE_QUESTS.Controllers // Make sure this namespace is correct
{
    public class HomeController : Controller
    {
        // This is your public landing page
        public IActionResult Index()
        {
            return View();
        }

        // ADD THIS METHOD TO FIX THE ERROR
        [Authorize] // This secures your dashboard
        public IActionResult Dashboard()
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