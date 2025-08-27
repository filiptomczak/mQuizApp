using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IQuizService:IBaseService<Quiz>
    {
        public Task<TakeTestVM?> GetTestVMWithQuestionsAndAnswersAsync(int id);
        public Task<IEnumerable<Quiz>> GetAllWithQuestionsAsync();
        public Task<Quiz> GetByIdWithQuestionsAndAnswersAsync(int id);
        public Task UpdateQuizAsync(QuizVM quizVM);
        public Task CreateNewQuizAsync(QuizVM quizVM);
        public Task DeleteQuestion(int id);
        public Task DeleteAnswer(int id);
        public Task DeleteImage(int id);
        public Task<bool> DeleteAsync(int id);
    }
}
