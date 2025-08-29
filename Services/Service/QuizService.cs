using DataAccess.IRepo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IQuestionService _questionService;
        private readonly IBaseService<Answer> _answerService;

        public QuizService(
            IUnitOfWork unitOfWork,
            IFileService fileService,
            IQuestionService questionService,
            IBaseService<Answer> answerService) : base(unitOfWork.Quizzes)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _questionService = questionService;
            _answerService = answerService;
        }

        public Task<TakeTestVM?> GetTestVMWithQuestionsAndAnswersAsync(int id)
        {
            return _unitOfWork.Quizzes.GetTestVMWithQuestionsAndAnswersAsync(id);
        }

        public async Task<IEnumerable<Quiz>> GetAllWithQuestionsAsync()
        {
            var quizes = await _unitOfWork.Quizzes.GetAllAsync();
            var questions = await _questionService.GetAllAsync();
            foreach (var quiz in quizes)
            {
                //quiz.Questions = questions.Where(q => q.QuizId == quiz.Id).ToList();
            }
            return quizes;
        }

        public async Task<Quiz> GetByIdWithQuestionsAsync(int id)
        {
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(id);

            if (quiz == null)
                return null;

            var questions = await _questionService.GetAllAsync();
            var answers = await _answerService.GetAllAsync();

            // tylko te pytania, które należą do quizu
            var filteredQuestions = questions.Where(q => q.QuizId == quiz.Id).ToList();

            foreach (var q in filteredQuestions)
            {
                if (q is SingleChoiceQuestion scq)
                {
                    scq.Answers = answers.Where(a => a.QuestionId == scq.Id).ToList();
                }
                else if (q is MatchQuestion mq)
                {
                    // tutaj np. PairService.GetAllByQuestionId(mq.Id)
                    mq.Pairs = new List<MatchPair>();
                }
                else if (q is OpenQuestion oq)
                {
                    // open question nie ma answers → nic nie robimy
                }
            }

            quiz.Questions = filteredQuestions.Cast<QuestionBase>().ToList();

            return quiz;
        }
        public async Task UpdateQuizAsync(QuizVM quizVM)
        {
            var quizFromDb = _unitOfWork.Quizzes.Get(x => x.Id == quizVM.Quiz.Id, "Questions,Questions.Answers");

            if (quizFromDb == null)
                throw new Exception("Quiz not found");

            quizFromDb.Title = quizVM.Quiz.Title;
            quizFromDb.Description = quizVM.Quiz.Description;

            foreach (var questionVM in quizVM.Questions)
            {
                var questionFromDb = quizFromDb.Questions?.FirstOrDefault(q => q.Id == questionVM.Id);

                if (questionFromDb != null)
                {
                    questionFromDb.Text = questionVM.Text;
                    if (questionVM.UploadedFile!=null)
                    {
                        if (!string.IsNullOrEmpty(questionFromDb.PathToFile))
                        {
                            _fileService.DeleteOld(questionFromDb.PathToFile);
                        }
                        questionFromDb.PathToFile = await _fileService.SaveFile(questionVM);
                    }

                    foreach (var answerVM in questionVM.Answers)
                    {
                        //var answerFromDb = questionFromDb.Answers.FirstOrDefault(a => a.Id == answerVM.Id);

                        //if (answerFromDb != null)
                        //{
                        //    answerFromDb.Text = answerVM.Text;
                        //    answerFromDb.IsCorrect = answerVM.IsCorrect;
                        //}
                        //else
                        //{
                        //    questionFromDb.Answers.Add(new Answer
                        //    {
                        //        Text = answerVM.Text,
                        //        IsCorrect = answerVM.IsCorrect
                        //    });
                        //}
                    }
                }
                else
                {
                    //quizFromDb.Questions?.Add(new Question
                    //{
                    //    Text = questionVM.Text,
                    //    Answers = questionVM.Answers.Select(a => new Answer
                    //    {
                    //        Text = a.Text,
                    //        IsCorrect = a.IsCorrect
                    //    }).ToList()
                    //});
                }
            }

            _unitOfWork.Quizzes.Update(quizFromDb);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateNewQuizAsync(QuizVM quizVM)
        {
            if (quizVM.Questions.Count != 0)
            {
                await AddQuestionsToQuizFromQuizVM(quizVM);
                
                _unitOfWork.Quizzes.Update(quizVM.Quiz);
                await _unitOfWork.CommitAsync();
            }
        }
        private async Task AddQuestionsToQuizFromQuizVM(QuizVM quizVM)
        {
            for (int i = 0; i < quizVM.Questions.Count; i++)
            {
                var questionVM = quizVM.Questions[i];
                var path = await _fileService.SaveFile(questionVM);

                //quizVM.Quiz?.Questions?.Add(new Question
                //{
                //    Text = questionVM.Text,
                //    PathToFile = path,
                //    QuizId = quizVM.Quiz.Id,
                //    Answers = questionVM.Answers.Select(a => new Answer
                //    {
                //        Text = a.Text,
                //        IsCorrect = a.IsCorrect
                //    }).ToList()
                //});
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var quiz = await GetByIdAsync(id);
            if (quiz == null)
                return false;

            Delete(quiz); // z BaseService
            await _unitOfWork.CommitAsync(); // teraz zatwierdzam
            return true;
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
                await _fileService.DeleteOld(entity.PathToFile);
                entity.PathToFile = null;
            }
            await _unitOfWork.CommitAsync();
        }
    }
}
