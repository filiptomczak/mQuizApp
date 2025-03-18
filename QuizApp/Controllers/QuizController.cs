using DataAccess.IRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ViewModels;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public QuizController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var quizes = await _unitOfWork.Quizzes.GetAllAsync();
            var questions = await _unitOfWork.Questions.GetAllAsync();
            foreach (var quiz in quizes) {
                quiz.Questions = questions.Where(q => q.QuizId == quiz.Id).ToList();
            }
            return View(quizes);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if(id == 0)
            {
                return View(new QuizVM());
            }
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(id);
            var questionsAll = await _unitOfWork.Questions.GetAllAsync();
            var questionsToQuiz = questionsAll.Where(x=>x.QuizId==quiz.Id).ToList();
            var quizVM = new QuizVM()
            {
                Quiz = quiz,
                Questions = questionsToQuiz
            };
            return View(quizVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(QuizVM quizVM)
        {
            if (ModelState.IsValid)
            {
                var quiz = new Quiz()
                {
                    Id = quizVM.Quiz.Id,
                    Title = quizVM.Quiz.Title,
                    Description = quizVM.Quiz.Description,
                    Questions = quizVM.Questions.Select(q => new Question
                    {
                        Text = q.Text,
                        TypeOfQuestion = q.TypeOfQuestion,
                        QuizId = quizVM.Quiz.Id, 
                        Answers = q.Answers.Select(a => new Answer
                        {
                            Text = a.Text,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    }).ToList()
                };
                _unitOfWork.Quizzes.Update(quiz);
                //_unitOfWork.Questions.UpdateRange(quizVM.Questions);
                return RedirectToAction(nameof(Index));
            }
            return View("Update",quizVM.Quiz.Id);
        }


        #region api
        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            var entity = await _unitOfWork.Quizzes.GetByIdAsync(id);
            _unitOfWork.Quizzes.Delete(entity);
            return Json(new
            {
                success = true,
                message = "Quiz Succesfully Deleted",
            });
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var entity = await _unitOfWork.Questions.GetByIdAsync(id);
            _unitOfWork.Questions.Delete(entity);
            return Json(new
            {
                success = true,
                message = "Question Succesfully Deleted",
            });
        }
        #endregion
    }
}
