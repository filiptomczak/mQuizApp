using DataAccess.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ViewModels;
using System.IO;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class QuizController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QuizController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var quizes = await _unitOfWork.Quizzes.GetAllAsync();
            var questions = await _unitOfWork.Questions.GetAllAsync();
            foreach (var quiz in quizes)
            {
                quiz.Questions = questions.Where(q => q.QuizId == quiz.Id).ToList();
            }
            return View(quizes);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id == 0)
            {
                var newQuiz = new QuizVM()
                {
                    Quiz = new Quiz()
                };
                return View(newQuiz);
            }
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(id);
            var questionsAll = await _unitOfWork.Questions.GetAllAsync();
            var questionsToQuiz = questionsAll.Where(x => x.QuizId == quiz.Id).ToList();
            var answersAll = await _unitOfWork.Answers.GetAllAsync();

            foreach (var question in questionsToQuiz)
            {
                question.Answers = answersAll.Where(x => x.QuestionId == question.Id).ToList();
            }

            var quizVM = new QuizVM()
            {
                Quiz = quiz,
                //Questions = questionsToQuiz,
                Questions = new List<QuestionVM>()
            };
            foreach (var question in questionsToQuiz)
            {
                quizVM.Questions.Add(new QuestionVM
                {
                    Answers = question.Answers,
                    Id = question.Id,
                    PathToFile = question.PathToFile,
                    Text = question.Text,
                    QuizId = question.QuizId,
                });
            }
            return View(quizVM);
        }

        private void AddQuestionsToQuizFromQuizVM(QuizVM quizVM)
        {
            for (int i = 0; i < quizVM.Questions.Count; i++)
            {
                var questionVM = quizVM.Questions[i];
                var path = "";// SaveFile(questionVM, formFiles.ElementAtOrDefault(i));

                quizVM.Quiz.Questions = new List<Question>();
                quizVM.Quiz.Questions.Add(new Question
                {
                    Text = questionVM.Text,
                    PathToFile = path,
                    QuizId = quizVM.Quiz.Id,
                    Answers = questionVM.Answers.Select(a => new Answer
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(QuizVM quizVM)
        {
            var questions = quizVM.Questions.Where(x => !string.IsNullOrEmpty(x.Text)).ToList();
            quizVM.Questions = questions;
            if (ModelState.IsValid)
            {
                //nowy quiz
                if (quizVM.Quiz.Id == 0)
                {
                    if (quizVM.Questions.Count != 0)
                    {
                        AddQuestionsToQuizFromQuizVM(quizVM);
                    }
                    _unitOfWork.Quizzes.Update(quizVM.Quiz);
                    return RedirectToAction(nameof(Index));
                }

                //edycja quizu
                var quizFromDb = _unitOfWork.Quizzes.Get(x => x.Id == quizVM.Quiz.Id, "Questions,Questions.Answers");

                //brak quizu w db
                if (quizFromDb == null)
                {
                    return NotFound();
                }

                //update danych quizu
                quizFromDb.Title = quizVM.Quiz.Title;
                quizFromDb.Description = quizVM.Quiz.Description;

                //nowe lub update pytan
                foreach (var questionVM in quizVM.Questions)
                {
                    var questionFromDb = quizFromDb.Questions.FirstOrDefault(x => x.Id == questionVM.Id);
                    //pytanie jest w db -> aktualizajca
                    if (questionFromDb != null)
                    {
                        questionFromDb.Text = questionVM.Text;
                        if (questionVM.UploadedFile != null)
                        {
                            if (!string.IsNullOrEmpty(questionFromDb.PathToFile))
                            {
                                DeleteOld(questionFromDb.PathToFile);
                            }
                            questionFromDb.PathToFile = SaveFile(questionVM);
                        }
                        //update odpowiedzi
                        foreach (var answerVM in questionVM.Answers)
                        {
                            if (!string.IsNullOrEmpty(answerVM.Text))
                            {
                                var answerFromDb = questionFromDb.Answers.FirstOrDefault(a => a.Id == answerVM.Id);

                                // update odpowiedzi
                                if (answerFromDb != null)
                                {
                                    answerFromDb.Text = answerVM.Text;
                                    answerFromDb.IsCorrect = answerVM.IsCorrect;
                                }
                                // jesli nie ma odp w db, tworzymy nowa 
                                else
                                {
                                    questionFromDb.Answers.Add(new Answer
                                    {
                                        Text = answerVM.Text,
                                        IsCorrect = answerVM.IsCorrect
                                    });
                                }
                            }
                        }
                    }
                    //brak pytania w db -> nowe pytanie
                    else
                    {
                        var newQuestion = new Question
                        {
                            Text = questionVM.Text,
                            PathToFile = SaveFile(questionVM),
                            QuizId = quizVM.Quiz.Id,
                            Answers = questionVM.Answers.Select(a => new Answer
                            {
                                Text = a.Text,
                                IsCorrect = a.IsCorrect
                            }).ToList()
                        };
                        quizFromDb.Questions.Add(newQuestion);
                    }
                }
                _unitOfWork.Quizzes.Update(quizFromDb);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Update", quizVM.Quiz.Id);
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

        #region files

        private void DeleteOld(string filePath)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var oldImgPath = Path.Combine(
                            wwwRootPath, filePath.TrimStart('/'));

            if (System.IO.File.Exists(oldImgPath))
            {
                System.IO.File.Delete(oldImgPath);
            }
        }
        private string SaveFile(QuestionVM question)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (question.UploadedFile != null)
            {
                string fileName = DateTime.Now.Year + "_" +
                    DateTime.Now.Month + "_" +
                    DateTime.Now.Day + "_" +
                    DateTime.Now.Hour + "_" +
                    DateTime.Now.Minute + "_" +
                    DateTime.Now.Second + "_" +
                    question.UploadedFile.FileName;

                string questionPath = Path.Combine(wwwRootPath, @"images\question");

                if (!Directory.Exists(questionPath))
                {
                    Directory.CreateDirectory(questionPath);
                }


                try
                {
                    using (var fileStream = new FileStream(Path.Combine(questionPath, fileName), FileMode.Create))
                    {
                        question.UploadedFile.CopyTo(fileStream);
                    }
                    return $"/images/question/{fileName}";
                }
                catch (Exception ex)
                {
                    // logowanie błędu np. _logger.LogError(ex, "Błąd przy zapisie pliku");
                    return string.Empty;
                }
            }
            return string.Empty;
        }
        #endregion

        #region api
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
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
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var entity = await _unitOfWork.Questions.GetByIdAsync(id);
            if (!string.IsNullOrEmpty(entity.PathToFile))
            {
                DeleteOld(entity.PathToFile);
                entity.PathToFile = null;
            }
            await _unitOfWork.CommitAsync();
            return Json(new
            {
                success = true,
                message = "Image Succesfully Deleted",
            });
        }
        #endregion
    }
}
