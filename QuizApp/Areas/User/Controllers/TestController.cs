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
        private readonly IQuizService _quizService;
        private readonly ITestService _testService;
        public TestController(IQuizService quizService, ITestService testService, IUnitOfWork unitOfWork)
        {
            _quizService = quizService;
            _unitOfWork = unitOfWork;
            _testService = testService;
        }
        public async Task<IActionResult> Index()
        {
            var tests = await _quizService.GetAllAsync();
            return View(tests);
        }
        [HttpGet]
        public IActionResult TakeTest(int id)
        {
            var takeTestVM = _testService.CreateTest(id);
            if (takeTestVM == null)
            {
                RedirectToAction(nameof(Index));
            }
            return View(takeTestVM.Result);
        }

        [HttpPost]
        public IActionResult TakeTest(TestSubmissionVM model)
        {
            if (!ModelState.IsValid) {
                RedirectToAction("TakeTest", model.QuizId);
            }

            _testService.SaveResult(model);
            return RedirectToAction(nameof(Index));
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
