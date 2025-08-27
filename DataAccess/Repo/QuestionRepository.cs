using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class QuestionRepository : BaseRepository<QuestionBase>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<QuestionBase>> GetByQuizIdAsync(int quizId)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId)
                .ToListAsync();
        }
        public void UpdateRange(IEnumerable<QuestionBase> questions)
        {
            _context.Questions.UpdateRange(questions);
        }
    }
}