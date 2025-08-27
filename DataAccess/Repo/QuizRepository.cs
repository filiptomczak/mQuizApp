using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class QuizRepository : BaseRepository<Quiz>, IQuizRepository
    {
        public QuizRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Quiz> GetQuizWithQuestionsAndAnswersAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<TakeTestVM?> GetTestVMWithQuestionsAndAnswersAsync(int id)
        {
            return await _context.Quizzes
                .Where(q => q.Id == id)
                .Select(q => new TakeTestVM
                {
                    QuizId = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    Questions = q.Questions.Select(ques => new TakeQuestionVM
                    {
                        QuestionId = ques.Id,
                        Text = ques.Text,
                        ImgPath = ques.PathToFile,
                        Answers = ques.Answers.Select(a => a.Text).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        /*
         * public async Task<TakeTestVM> GetTestVMWithQuestionsAndAnswersAsync(int id)
        {
            return _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }
         */
    }
}
