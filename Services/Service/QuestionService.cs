using DataAccess.IRepo;
using Models.Models;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class QuestionService : BaseService<QuestionBase>, IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionService(IUnitOfWork unitOfWork):base(unitOfWork.Questions)
        {
            _unitOfWork=unitOfWork;
        }

        public async Task<int> AddMatchAsync(int quizId, string text, string? pathToFile, IEnumerable<(string imagePath, string label)> pairs)
        {
            var q = new MatchQuestion
            {
                QuizId = quizId,
                Text = text,
                PathToFile = pathToFile,
                Pairs = pairs.Select(p => new MatchPair { ImagePath = p.imagePath, Label = p.label }).ToList()
            };

            await _unitOfWork.Questions.AddAsync(q);
            await _unitOfWork.CommitAsync();
            return q.Id;
        }

        public async Task<int> AddOpenAsync(int quizId, string text, string? pathToFile, string? correctAnswer)
        {
            var q = new OpenQuestion
            {
                QuizId = quizId,
                Text = text,
                PathToFile = pathToFile,
                CorrectAnswer = correctAnswer
            };

            await _unitOfWork.Questions.AddAsync(q);
            await _unitOfWork.CommitAsync();
            return q.Id;
        }

        public async Task<int> AddSingleChoiceAsync(int quizId, string text, string? pathToFile, IEnumerable<(string text, bool isCorrect)> answers)
        {
            var q = new SingleChoiceQuestion
            {
                QuizId = quizId,
                Text = text,
                PathToFile = pathToFile,
                Answers = answers.Select(a => new Answer { Text = a.text, IsCorrect = a.isCorrect }).ToList()
            };

            // dodajemy do repo bazowego Questions
            await _unitOfWork.Questions.AddAsync(q);

            // zapis
            await _unitOfWork.CommitAsync();

            return q.Id;
        }
    }
}
