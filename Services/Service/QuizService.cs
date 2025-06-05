using DataAccess.IRepo;
using Microsoft.AspNetCore.Hosting;
using Models.Models;
using Models.ViewModels;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class QuizService : BaseService<Quiz>, IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IQuestionService _questionService;
        private readonly IBaseService<Answer> _answerService;

        public QuizService(IQuizRepository quizRepository, 
            IUnitOfWork unitOfWork,
            IFileService fileService,
            IQuestionService questionService,
            IBaseService<Answer> answerService) : base(quizRepository, unitOfWork)
        {
            _quizRepository = quizRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _questionService = questionService;
            _answerService = answerService;
        }

        public Quiz GetQuizWithQuestionsAndAnswers(int id)
        {
            return _quizRepository.GetQuizWithQuestionsAndAnswers(id);
        }

        public async Task<IEnumerable<Quiz>> GetAllWithQuestionsAsync()
        {
            var quizes = await _unitOfWork.Quizzes.GetAllAsync();
            var questions = await _questionService.GetAllAsync();
            foreach (var quiz in quizes)
            {
                quiz.Questions = questions.Where(q => q.QuizId == quiz.Id).ToList();
            }
            return quizes;
        }

        public async Task<Quiz> GetByIdWithQuestionsAsync(int id)
        {
            var quiz = await _quizRepository.GetByIdAsync(id);
            var questions = await _questionService.GetAllAsync();
            var answers = await _answerService.GetAllAsync();

            quiz.Questions = questions
                .Where(q => q.QuizId == quiz.Id)
                .Select(q =>
                {
                    q.Answers = answers.Where(a => a.QuestionId == q.Id).ToList();
                    return q;
                }).ToList();

            return quiz;
        }

        public async Task UpdateQuizAsync(QuizVM quizVM)
        {
            var quizFromDb = _quizRepository.Get(x => x.Id == quizVM.Quiz.Id, "Questions,Questions.Answers");

            if (quizFromDb == null)
                throw new Exception("Quiz not found");

            quizFromDb.Title = quizVM.Quiz.Title;
            quizFromDb.Description = quizVM.Quiz.Description;

            foreach (var questionVM in quizVM.Questions)
            {
                var questionFromDb = quizFromDb.Questions.FirstOrDefault(q => q.Id == questionVM.Id);

                if (questionFromDb != null)
                {
                    questionFromDb.Text = questionVM.Text;
                    if (questionVM.UploadedFile!=null)
                    {
                        if (!string.IsNullOrEmpty(questionFromDb.PathToFile))
                        {
                            _fileService.DeleteOld(questionFromDb.PathToFile);
                        }
                        questionFromDb.PathToFile = _fileService.SaveFile(questionVM);
                    }

                    foreach (var answerVM in questionVM.Answers)
                    {
                        var answerFromDb = questionFromDb.Answers.FirstOrDefault(a => a.Id == answerVM.Id);

                        if (answerFromDb != null)
                        {
                            answerFromDb.Text = answerVM.Text;
                            answerFromDb.IsCorrect = answerVM.IsCorrect;
                        }
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
                else
                {
                    quizFromDb.Questions.Add(new Question
                    {
                        Text = questionVM.Text,
                        Answers = questionVM.Answers.Select(a => new Answer
                        {
                            Text = a.Text,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    });
                }
            }

            _unitOfWork.Quizzes.Update(quizFromDb);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateNewQuizAsync(QuizVM quizVM)
        {
            if (quizVM.Questions.Count != 0)
            {
                AddQuestionsToQuizFromQuizVM(quizVM);
                
                _quizRepository.Update(quizVM.Quiz);
                await _unitOfWork.CommitAsync();
            }
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

        public async Task DeleteQuestion(int id)
        {
            var entity = await _questionService.GetByIdAsync(id);
            _questionService.Delete(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAnswer(int id)
        {
            var entity = await _answerService.GetByIdAsync(id);
            _answerService.Delete(entity);
            await _unitOfWork.CommitAsync();
        }
        public async Task DeleteImage(int id)
        {
            var entity = await _questionService.GetByIdAsync(id);
            if (!string.IsNullOrEmpty(entity.PathToFile))
            {
                _fileService.DeleteOld(entity.PathToFile);
                entity.PathToFile = null;
            }
            await _unitOfWork.CommitAsync();
        }
    }
}
