using System.Collections.Generic;

namespace CODE_QUESTS.Models
{
    // This holds all the info for one question screen
    public class QuestionViewModel
    {
        public string QuizId { get; set; } = string.Empty; // e.g., "C++" or "JAVA"
        public string QuizTitle { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public List<AnswerOption> Options { get; set; } = new List<AnswerOption>();

        // We'll use these later to add a "Next Question" button
        public int CurrentQuestionIndex { get; set; }
        public int TotalQuestions { get; set; }
    }

    // This holds a single answer choice
    public class AnswerOption
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}