using DataAccess.IRepo;
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
    public class ResultService : IResultService
    {
        private readonly IQuizService _quizService;
        private readonly IBaseService<TestResult> _testResultService;

        public ResultService(IQuizService quizService,
            IBaseService<TestResult> testResultService)
        {
            _quizService=quizService;
            _testResultService=testResultService;
        }
        public async Task<List<QuizLeaderboardVM>> GetAllQuizzesWithNResults(int n)
        {
            var quizzes = await _quizService.GetAllAsync();
            var allResults = await _testResultService.GetAllAsync();

            var leaderboard = quizzes.Select(q => new QuizLeaderboardVM
            {
                QuizId = q.Id,
                Title = q.Title,
                Results = GetNResults(allResults,q.Id,n)
            });
            return leaderboard.ToList();
        }

        public async Task<QuizLeaderboardVM> GetQuizzWithAllResults(int quizId, int n = -1)
        {
            var quizz = await _quizService.GetByIdAsync(quizId);
            var allResults = await _testResultService.GetAllAsync();

            var quiz = new QuizLeaderboardVM
            {
                QuizId = quizz.Id,
                Title = quizz.Title,
                Results = GetNResults(allResults, quizId, n)
            };

            return quiz;
        }

        private List<TestResult> GetNResults(IEnumerable<TestResult> allResults,int quizId, int n)
        {
            var results = allResults
                    .Where(t => t.QuizId == quizId)
                    .OrderByDescending(t => t.Points);
                    

            return n == -1 ? results.ToList() : results.Take(n).ToList();
        }
    }
}
