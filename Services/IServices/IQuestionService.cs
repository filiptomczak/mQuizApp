using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IQuestionService:IBaseService<QuestionBase>
    {
        Task<int> AddSingleChoiceAsync(int quizId, string text, string? pathToFile, IEnumerable<(string text, bool isCorrect)> answers);
        Task<int> AddMatchAsync(int quizId, string text, string? pathToFile, IEnumerable<(string imagePath, string label)> pairs);
        Task<int> AddOpenAsync(int quizId, string text, string? pathToFile, string? correctAnswer);
    }
}
