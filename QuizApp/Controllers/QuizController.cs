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
            var answersAll = await _unitOfWork.Answers.GetAllAsync();

            foreach (var question in questionsToQuiz) {
                question.Answers = answersAll.Where(x=>x.QuestionId==question.Id).ToList();
            }
            
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
                var quizFromDb = _unitOfWork.Quizzes.Get(x=>x.Id == quizVM.Quiz.Id, "Questions,Questions.Answers");
                if (quizFromDb == null)
                {
                    return NotFound();
                }

                quizFromDb.Title = quizVM.Quiz.Title;
                quizFromDb.Description = quizVM.Quiz.Description;

                foreach (var questionVM in quizVM.Questions)
                {
                    var questionFromDb = quizFromDb.Questions.FirstOrDefault(x => x.Id == questionVM.Id);
                    if (questionFromDb != null)
                    {

                        questionFromDb.Text = questionVM.Text;
                        questionFromDb.TypeOfQuestion = questionVM.TypeOfQuestion;
                        questionFromDb.PathToFile = questionVM.PathToFile;


                        foreach (var answerVM in questionVM.Answers)
                        {
                            var answerFromDb = questionFromDb.Answers.FirstOrDefault(a => a.Id == answerVM.Id);
                            if (answerFromDb != null)
                            {
                                // Jeśli odpowiedź istnieje -> aktualizuj
                                answerFromDb.Text = answerVM.Text;
                                answerFromDb.IsCorrect = answerVM.IsCorrect;
                            }
                            else
                            {
                                // Jeśli odpowiedzi NIE ma -> dodaj nową
                                questionVM.Answers.Add(new Answer
                                {
                                    Text = answerVM.Text,
                                    IsCorrect = answerVM.IsCorrect
                                });
                            }
                        }
                    }
                    else
                    {
                        quizFromDb.Questions.Add(new Question
                        {
                            Text = questionVM.Text,
                            TypeOfQuestion = questionVM.TypeOfQuestion,
                            PathToFile = questionVM.PathToFile,
                            QuizId = quizVM.Quiz.Id,
                            Answers = questionVM.Answers.Select(a => new Answer
                            {
                                Text = a.Text,
                                IsCorrect = a.IsCorrect
                            }).ToList()
                        });
                    }
                }
                _unitOfWork.Quizzes.Update(quizFromDb);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Update",quizVM.Quiz.Id);
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

        [HttpDelete]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var entity = await _unitOfWork.Answers.GetByIdAsync(id);
            _unitOfWork.Answers.Delete(entity);
            return Json(new
            {
                success = true,
                message = "Answer Succesfully Deleted",
            });
        }
        #endregion
    }
}
