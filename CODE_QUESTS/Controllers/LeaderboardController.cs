using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CODE_QUESTS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CODE_QUESTS.Controllers
{
    [Authorize] // This page requires a user to be logged in
    public class LeaderboardController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public LeaderboardController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // --- This is temporary data. ---
            // In your real app, you would fetch this from your database.
            var fakeScores = new Dictionary<string, (int Score, int Quests)>
            {
                { "CodeWarrior", (7000, 50) },
                { "ByteNinja", (6000, 48) },
                { "SyntaxSlayer", (5000, 46) },
                { "BugHunterX", (4000, 44) },
                { "LoopMaster", (3000, 42) },
                { "AlphaDev", (2000, 40) },
                { "BetaByte", (1000, 38) }
            };

            // Get the currently logged-in user
            var currentUser = await _userManager.GetUserAsync(User);

            // Add the current user to the fake score list if they aren't there
            if (!fakeScores.ContainsKey(currentUser.UserName))
            {
                fakeScores[currentUser.UserName] = (300, 5); // Give them a sample score
            }

            // Create the list of leaderboard entries
            var entries = fakeScores
                .OrderByDescending(pair => pair.Value.Score) // Sort by score
                .Select((pair, index) => new LeaderboardEntry
                {
                    Rank = index + 1,
                    Username = pair.Key,
                    TotalScore = pair.Value.Score,
                    QuestsCompleted = pair.Value.Quests
                })
                .ToList();

            // Find the current user's rank and score
            var currentUserData = entries.FirstOrDefault(e => e.Username == currentUser.UserName);

            // Create the final ViewModel to send to the page
            var model = new LeaderboardViewModel
            {
                CurrentUserRank = currentUserData?.Rank ?? 0,
                CurrentUserScore = currentUserData?.TotalScore ?? 0,
                Entries = entries
            };

            return View(model);
        }
    }
}