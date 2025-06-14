﻿using Models.Models;
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
        public Quiz GetQuizWithQuestionsAndAnswers(int id);
        public Task<IEnumerable<Quiz>> GetAllWithQuestionsAsync();
        public Task<Quiz> GetByIdWithQuestionsAsync(int id);
        public Task UpdateQuizAsync(QuizVM quizVM);
        public Task CreateNewQuizAsync(QuizVM quizVM);
        public Task DeleteQuestion(int id);
        public Task DeleteAnswer(int id);
        public Task DeleteImage(int id);
    }
}
