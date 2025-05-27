using DataAccess.IRepo;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace QuizApp.Areas.Admin
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
                TopResults = _unitOfWork.TestResults.GetAllAsync().Result
                    .Where(t => t.QuizId == q.Id)
                    .OrderByDescending(t => t.Points)
                    .Take(3)
                    .ToList(),
            }).ToList();

            return View(leadboarders);
        }
    }
}
