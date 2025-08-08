using DataAccess.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ViewModels;
using Services.IServices;
using System.IO;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public async Task<IActionResult> Index()
        {
            var quizes = await _quizService.GetAllWithQuestionsAsync();
            return View(quizes);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id == 0)
                return View(new QuizVM {
                    Quiz = new Quiz(), 
                    Questions = new List<QuestionVM>() 
                });

            var quiz = await _quizService.GetByIdWithQuestionsAsync(id);

            var quizVM = new QuizVM()
            {
                Quiz = quiz,
                Questions = quiz.Questions.Select(q => new QuestionVM
                {
                    Id = q.Id,
                    PathToFile = q.PathToFile,
                    Text = q.Text,
                    QuizId = q.QuizId,
                    Answers = q.Answers,
                }).ToList()
            };
            return View(quizVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(QuizVM quizVM)
        {
            quizVM.Questions = quizVM.Questions.Where(x => !string.IsNullOrEmpty(x.Text)).ToList();

            if (!ModelState.IsValid)
                return View(quizVM);

            //nowy quiz
            if (quizVM.Quiz.Id == 0)
            {
                await _quizService.CreateNewQuizAsync(quizVM);
            }
            else
            {
                await _quizService.UpdateQuizAsync(quizVM);

            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetQuestionForm(int index)
        {
            var newQuestion = new QuestionVM
            {
                Id = index,
                Answers = new List<Answer>()
            };

            ViewData.TemplateInfo.HtmlFieldPrefix = $"Questions[{index}]";
            return PartialView("_QuestionForm", newQuestion);
        }
        
        #region api
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _quizService.DeleteAsync(id))
            {
                return Json(new
                {
                    success = true,
                    message = "Quiz Succesfully Deleted",
                });
            }
            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            await _quizService.DeleteQuestion(id);
            return Json(new
            {
                success = true,
                message = "Question Succesfully Deleted",
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            await _quizService.DeleteAnswer(id);
            return Json(new
            {
                success = true,
                message = "Answer Succesfully Deleted",
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int id)
        {
            await _quizService.DeleteImage(id);
            return Json(new
            {
                success = true,
                message = "Image Succesfully Deleted",
            });
        }
        #endregion
    }
}
