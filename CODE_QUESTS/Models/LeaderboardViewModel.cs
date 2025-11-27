using System.Collections.Generic;

namespace CODE_QUESTS.Models
{
    // This model holds ALL the data for the leaderboard page
    public class LeaderboardViewModel
    {
        public int CurrentUserRank { get; set; }
        public int CurrentUserScore { get; set; }
        public List<LeaderboardEntry> Entries { get; set; } = new List<LeaderboardEntry>();
    }

    // This represents a single row in the leaderboard
    public class LeaderboardEntry
    {
        public int Rank { get; set; }
        public string Username { get; set; } = string.Empty;
        public int TotalScore { get; set; }
        public int QuestsCompleted { get; set; }
    }
}