using DataAccess.IRepo;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels;
using Services.IServices;

namespace QuizApp.Areas.User.Controllers
{
    [Area("User")]
    public class TestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestController(IUnitOfWork unitOfWork)
        {
                _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var tests = await _unitOfWork.Quizzes.GetAllAsync();
            return View(tests);
        }
        [HttpGet]
        public IActionResult TakeTest(int id)
        {
            var quiz = _unitOfWork.Quizzes.GetQuizWithQuestionsAndAnswers(id);
            if(quiz == null)
            {
                RedirectToAction("Index");
            }

            var takeTestVM = new TakeTestVM
            {
                QuizId = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Questions = quiz.Questions.Select(q => new TakeQuestionVM
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    ImgPath = q.PathToFile,
                    Answers = q.Answers.Select(a => a.Text).ToList()
                }).ToList()
            };

            return View(takeTestVM);
        }

        [HttpPost]
        public IActionResult TakeTest(TestSubmissionVM model)
        {
            if (!ModelState.IsValid) {
                RedirectToAction("TakeTest", model.QuizId);
            }

            var points = CheckAnswers(model.Answers);
            SaveResultAsync(points,model.UserName,model.QuizId);
            return RedirectToAction(nameof(Index));
        }

        private int CheckAnswers(List<SubmittedAnswerVM>answers)
        {
            var result = 0;
            foreach (var answer in answers) {
                var questionId = answer.QuestionId;
                var correctAnswerText = _unitOfWork.Questions
                        .Get(q => q.Id == questionId, includeProperties:"Answers")?
                        .Answers.SingleOrDefault(a => a.IsCorrect)?
                        .Text;

                if (correctAnswerText == answer.SelectedAnswer)
                    result++;
            }
            return result;
            //zapis username, quiz name/id i result do db

        }
        private async Task SaveResultAsync(int points, string userName, int quizId)
        {
            var testResult = new TestResult()
            {
                UserName = userName,
                QuizId = quizId,
                Points = points
            };
            await _unitOfWork.TestResults.AddAsync(testResult);
            await _unitOfWork.CommitAsync();
        }
    }
}
