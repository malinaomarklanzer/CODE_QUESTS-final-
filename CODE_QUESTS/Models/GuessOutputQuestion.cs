namespace CODE_QUESTS.Models
{
    public class GuessOutputQuestion
    {
        public string QuizTitle { get; set; } = "Guess the Output";
        public string CodeProblem { get; set; } = string.Empty;
        public string CorrectOutput { get; set; } = string.Empty;
    }
}