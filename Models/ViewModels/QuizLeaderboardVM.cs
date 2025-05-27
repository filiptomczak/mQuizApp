using Models.Models;

namespace Models.ViewModels

{
    public class QuizLeaderboardVM
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public IEnumerable<TestResult> TopResults { get; set; }
    }
}