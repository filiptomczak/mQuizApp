using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;
using Models.Models;
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

        public Quiz GetQuizWithQuestionsAndAnswers(int id)
        {
            return _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefault(q => q.Id == id);
        }
    }
}
