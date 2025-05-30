using DataAccess.IRepo;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ResultController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResultController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var quizzes = _unitOfWork.Quizzes.GetAllAsync();

            var leadboarders = quizzes.Result.Select(q => new QuizLeaderboardVM
            {
                QuizId = q.Id,
                Title = q.Title,
                Results = _unitOfWork.TestResults.GetAllAsync().Result
                    .Where(t => t.QuizId == q.Id)
                    .OrderByDescending(t => t.Points)
                    .Take(3)
                    .ToList(),
            }).ToList();

            return View(leadboarders);
        }

        public async Task<IActionResult> Details()
        {
            var quizzes = _unitOfWork.Quizzes.GetAllAsync();

            var results = quizzes.Result.Select(q => new QuizLeaderboardVM
            {
                QuizId = q.Id,
                Title = q.Title,
                Results = _unitOfWork.TestResults.GetAllAsync().Result
                    .Where(t => t.QuizId == q.Id)
                    .OrderByDescending(t => t.Points)
                    .ToList(),
            }).ToList();

            return View(results);
        }
    }
}
