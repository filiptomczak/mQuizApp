using DataAccess.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Services.IServices;
using Services.Service;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ResultController : Controller
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }
        public async Task<IActionResult> Index()
        {
            var top3results = _resultService.GetAllQuizzesWithNResults(3);

            return View(top3results.Result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var allResults = _resultService.GetQuizzWithAllResults(id);

            return View(allResults.Result);
        }
    }
}
