using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IResultService
    {
        public Task<List<QuizLeaderboardVM>> GetAllQuizzesWithNResults(int n);
        public Task<QuizLeaderboardVM> GetQuizzWithAllResults(int quizId, int n = -1);
    }
}
