using DataAccess.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ViewModels;
using Services.IServices;
using System.IO;
using System.Text;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IQuizExportService _quizExportService;

        public QuizController(IQuizService quizService, IQuizExportService quizExportService)
        {
            _quizService = quizService;
            _quizExportService = quizExportService;
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
                Questions = quiz.Questions.Select(q =>
                {
                    var qvm = new QuestionVM
                    {
                        Id = q.Id,
                        PathToFile = q.PathToFile,
                        Text = q.Text,
                        QuizId = q.QuizId,
                        Type = q switch
                        {
                            SingleChoiceQuestion => QuestionType.SingleChoice,
                            OpenQuestion => QuestionType.Open,
                            MatchQuestion => QuestionType.Match,
                            _ => throw new NotSupportedException($"Unknown question type: {q.GetType().Name}")
                        } // np. "SingleChoiceQuestion"
                    };

                    switch (q)
                    {
                        case SingleChoiceQuestion scq:
                            qvm.Answers = scq.Answers?
                                .Select(a => new AnswerVM
                                {
                                    AnswerId = a.Id,
                                    Text = a.Text,
                                    IsCorrect = a.IsCorrect
                                })
                                .ToList()
                                ?? new List<AnswerVM>();
                            break;

                        case MatchQuestion mq:
                            qvm.Pairs = mq.Pairs?.Select(p => new PairVM
                            {
                                ImagePath = p.ImagePath,
                                Label = p.Label
                            }).ToList();
                            break;

                        case OpenQuestion oq:
                            qvm.CorrectAnswer = oq.CorrectAnswer;
                            break;
                    }

                    return qvm;
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

        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            var quiz = await _quizService.GetByIdWithQuestionsAsync(id);
            if(quiz==null)
                return RedirectToAction(nameof(Index));

            var quizTxt = _quizExportService.ExportQuizAsText(quiz);
            if(quizTxt==null) 
                return RedirectToAction(nameof(Index));
            var fileName = $"{quiz.Title}.txt";
            var bytes= Encoding.UTF8.GetBytes(quizTxt);

            return File(bytes, "text/plain", fileName);
        }

        public IActionResult GetQuestionForm(int index, string type)
        {
            var newQuestion = new QuestionVM
            {
                Id = index,
                Answers = new List<AnswerVM>(),  // <-- zmienione na AnswerVM
                Pairs = new List<PairVM>(),      // też warto od razu zainicjalizować
                Type = QuestionType.SingleChoice // domyślny typ np. SingleChoice
            };

            ViewData.TemplateInfo.HtmlFieldPrefix = $"Questions[{index}]";
            return type switch
            {
                "SingleChoiceQuestion" => PartialView("_SingleChoiceQuestionForm", newQuestion),
                "MatchQuestion" => PartialView("_MatchQuestionForm", newQuestion),
                "OpenQuestion" => PartialView("_OpenQuestionForm", newQuestion),
                _ => PartialView("_OpenQuestionForm", newQuestion)
            };
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
