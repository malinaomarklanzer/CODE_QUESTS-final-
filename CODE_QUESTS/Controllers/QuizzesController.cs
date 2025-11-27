using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CODE_QUESTS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CODE_QUESTS.Controllers
{
    [Authorize]
    public class QuizzesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public QuizzesController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // --- SCORE CHECK ---
        // Set to 0 to see everything LOCKED initially.
        // Set to 3000 to unlock Beginner Debugging.
        // Set to 7000 to unlock Intermediate.
        private async Task<int> GetCurrentUserScoreAsync()
        {
            return 0; // <--- CHANGE THIS TO TEST UNLOCKS
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            int userScore = 0;
            if (User.Identity?.IsAuthenticated == true)
            {
                userScore = await GetCurrentUserScoreAsync();
            }
            ViewData["UserScore"] = userScore;
            return View();
        }

        // --- CINEMATIC ENTRY POINT ---
        public IActionResult GameStart(string id)
        {
            // This starts the story at Scene 1 for the chosen Tier (id)
            return RedirectToAction("IntroScene", new { scene = 1, quizId = id });
        }

        [AllowAnonymous]
        public IActionResult IntroScene(int scene, string quizId)
        {
            ViewData["SceneNumber"] = scene;
            ViewData["QuizId"] = quizId; // Keep track of which tier we are playing
            return View();
        }

        // --- GAME ACTIONS ---
        public IActionResult PlayMultipleChoice(string id)
        {
            var questions = QuestionBank.GetMultipleChoice(id);
            ViewData["QuizTitle"] = $"{id.ToUpper()}: MULTIPLE CHOICE";
            return View("PlayMultipleChoice", questions); // questions list will be empty if invalid, handled in view
        }

        public IActionResult PlayDebugging(string id)
        {
            var questions = QuestionBank.GetDebugging(id);
            ViewData["QuizTitle"] = $"{id.ToUpper()}: DEBUGGING";
            return View("PlayDebugging", questions);
        }

        public IActionResult PlayGuessOutput(string id)
        {
            var questions = QuestionBank.GetGuessOutput(id);
            ViewData["QuizTitle"] = $"{id.ToUpper()}: GUESS THE OUTPUT";
            return View("PlayGuessOutput", questions);
        }
    }

    // ==================================================================
    // --- QUESTION BANK (TIER BASED) ---
    // ==================================================================
    public static class QuestionBank
    {
        public static List<QuestionViewModel> GetMultipleChoice(string tier)
        {
            var questions = new List<QuestionViewModel>();

            if (tier == "Beginner") // HTML + CSS
            {
                questions.Add(new QuestionViewModel { QuestionText = "What does HTML stand for?", Options = new List<AnswerOption> { new AnswerOption { Text = "Hyper Text Markup Language", IsCorrect = true }, new AnswerOption { Text = "Home Tool Markup Language", IsCorrect = false }, new AnswerOption { Text = "Hyperlinks and Text Markup Language", IsCorrect = false }, new AnswerOption { Text = "Hyper Tooling Markup Language", IsCorrect = false } } });
                questions.Add(new QuestionViewModel { QuestionText = "What does CSS stand for?", Options = new List<AnswerOption> { new AnswerOption { Text = "Cascading Style Sheets", IsCorrect = true }, new AnswerOption { Text = "Creative Style Sheets", IsCorrect = false }, new AnswerOption { Text = "Computer Style Sheets", IsCorrect = false }, new AnswerOption { Text = "Colorful Style Sheets", IsCorrect = false } } });
                questions.Add(new QuestionViewModel { QuestionText = "Which tag is used to define an unordered list?", Options = new List<AnswerOption> { new AnswerOption { Text = "<ul>", IsCorrect = true }, new AnswerOption { Text = "<ol>", IsCorrect = false }, new AnswerOption { Text = "<li>", IsCorrect = false }, new AnswerOption { Text = "<list>", IsCorrect = false } } });
                questions.Add(new QuestionViewModel { QuestionText = "Which HTML attribute is used to define inline styles?", Options = new List<AnswerOption> { new AnswerOption { Text = "class", IsCorrect = false }, new AnswerOption { Text = "style", IsCorrect = true }, new AnswerOption { Text = "font", IsCorrect = false }, new AnswerOption { Text = "styles", IsCorrect = false } } });
                questions.Add(new QuestionViewModel { QuestionText = "Which property is used to change the background color?", Options = new List<AnswerOption> { new AnswerOption { Text = "color", IsCorrect = false }, new AnswerOption { Text = "bgcolor", IsCorrect = false }, new AnswerOption { Text = "background-color", IsCorrect = true }, new AnswerOption { Text = "background", IsCorrect = false } } });
            }
            else if (tier == "Intermediate") // Java + C#
            {
                questions.Add(new QuestionViewModel { QuestionText = "Which of the following correctly defines a class in Java?", Options = new List<AnswerOption> { new AnswerOption { Text = "define class MyClass {}", IsCorrect = false }, new AnswerOption { Text = "class MyClass {}", IsCorrect = true }, new AnswerOption { Text = "MyClass class {}", IsCorrect = false }, new AnswerOption { Text = "public define MyClass {}", IsCorrect = false } } });
                questions.Add(new QuestionViewModel { QuestionText = "In C#, which keyword is used to handle exceptions?", Options = new List<AnswerOption> { new AnswerOption { Text = "try-catch", IsCorrect = true }, new AnswerOption { Text = "if-else", IsCorrect = false }, new AnswerOption { Text = "switch", IsCorrect = false }, new AnswerOption { Text = "throw-catch", IsCorrect = false } } });
                questions.Add(new QuestionViewModel { QuestionText = "Which of these data types is not primitive in Java?", Options = new List<AnswerOption> { new AnswerOption { Text = "int", IsCorrect = false }, new AnswerOption { Text = "float", IsCorrect = false }, new AnswerOption { Text = "String", IsCorrect = true }, new AnswerOption { Text = "boolean", IsCorrect = false } } });
                questions.Add(new QuestionViewModel { QuestionText = "In C#, what is the correct syntax to declare a list of integers?", Options = new List<AnswerOption> { new AnswerOption { Text = "List<int> numbers = new List<int>();", IsCorrect = true }, new AnswerOption { Text = "int[] numbers = new List<int>();", IsCorrect = false }, new AnswerOption { Text = "List numbers = new List<int>();", IsCorrect = false }, new AnswerOption { Text = "list<int> numbers = new list<int>();", IsCorrect = false } } });
            }
            else if (tier == "Professional") // JS + C++
            {
                questions.Add(new QuestionViewModel { QuestionText = "What is the main difference between == and === in JavaScript?", Options = new List<AnswerOption> { new AnswerOption { Text = "=== is for assignment, == is for comparison.", IsCorrect = false }, new AnswerOption { Text = "== compares only value, === compares both value and type.", IsCorrect = true }, new AnswerOption { Text = "== is for strings, === is for numbers.", IsCorrect = false }, new AnswerOption { Text = "There is no difference.", IsCorrect = false } } });
                questions.Add(new QuestionViewModel { QuestionText = "Which of the following is a correct way to define a class in C++?", Options = new List<AnswerOption> { new AnswerOption { Text = "object MyClass {}", IsCorrect = false }, new AnswerOption { Text = "define class MyClass {}", IsCorrect = false }, new AnswerOption { Text = "MyClass class {}", IsCorrect = false }, new AnswerOption { Text = "class MyClass {}", IsCorrect = true } } });
                questions.Add(new QuestionViewModel { QuestionText = "How do you create a function in JavaScript?", Options = new List<AnswerOption> { new AnswerOption { Text = "function myFunction()", IsCorrect = true }, new AnswerOption { Text = "function:myFunction()", IsCorrect = false }, new AnswerOption { Text = "function = myFunction()", IsCorrect = false }, new AnswerOption { Text = "def myFunction()", IsCorrect = false } } });
            }
            return questions;
        }

        public static List<DebuggingQuestion> GetDebugging(string tier)
        {
            var questions = new List<DebuggingQuestion>();

            if (tier == "Beginner")
            {
                questions.Add(new DebuggingQuestion { CodeProblem = "<p>This is a <strong>bold paragraph.</p>", CodeSolution = "</strong>" });
                questions.Add(new DebuggingQuestion { CodeProblem = ".title {\n    font-size: 20px\n    color: blue;\n}", CodeSolution = ";" });
                questions.Add(new DebuggingQuestion { CodeProblem = "<img src='image.png' alt='My Image'>", CodeSolution = "<img>" });
            }
            else if (tier == "Intermediate")
            {
                questions.Add(new DebuggingQuestion { CodeProblem = "public class MyClass {\n    public static void main(String[] args) {\n        String name = \"CodeQuest\";\n        System.out.println(name.length)\n    }\n}", CodeSolution = "()" });
                questions.Add(new DebuggingQuestion { CodeProblem = "using System;\nclass Program {\n    static void Main(string[] args) {\n        string word = \"Hello\"\n        Console.WriteLine(word);\n    }\n}", CodeSolution = ";" });
            }
            else if (tier == "Professional")
            {
                questions.Add(new DebuggingQuestion { CodeProblem = "let user = {\n    name: \"Alex\",\n    age: 30\n}\nconsole.log(user.Age);", CodeSolution = "age" });
                questions.Add(new DebuggingQuestion { CodeProblem = "for (int i = 0; i < 5; i++) {\n    cout << ____ << endl;\n}", CodeSolution = "i" });
            }
            return questions;
        }

        public static List<GuessOutputQuestion> GetGuessOutput(string tier)
        {
            var questions = new List<GuessOutputQuestion>();

            if (tier == "Beginner")
            {
                questions.Add(new GuessOutputQuestion { CodeProblem = "This text is <b>bold</b> and this is <i>italic</i>.", CorrectOutput = "This text is bold and this is italic." });
                questions.Add(new GuessOutputQuestion { CodeProblem = "<div style=\"color: red;\">Red Text</div>", CorrectOutput = "Red Text" });
            }
            else if (tier == "Intermediate")
            {
                questions.Add(new GuessOutputQuestion { CodeProblem = "System.out.println(\"Java\" + 10 + 20);", CorrectOutput = "Java1020" });
                questions.Add(new GuessOutputQuestion { CodeProblem = "int x = 10;\nint y = 5;\nstring result = (x > y) ? \"Greater\" : \"Lesser\";\nConsole.WriteLine(result);", CorrectOutput = "Greater" });
            }
            else if (tier == "Professional")
            {
                questions.Add(new GuessOutputQuestion { CodeProblem = "console.log(1 + '2' + 3);", CorrectOutput = "123" });
                questions.Add(new GuessOutputQuestion { CodeProblem = "int a = 5;\nint b = a++ + a;\nstd::cout << b << std::endl;", CorrectOutput = "11" });
            }
            return questions;
        }
    }
}