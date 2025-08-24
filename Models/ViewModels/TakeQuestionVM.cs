using Models.Models;

namespace Models.ViewModels
{

    public class TakeQuestionVM
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? ImgPath { get; set; }

        public QuestionType Type { get; set; }
        // tylko dla SingleChoice
        public List<string>? Answers { get; set; }
        // tylko dla Match
        public List<MatchPairVM>? Pairs { get; set; }
        // tylko dla Open
        public string? Placeholder { get; set; } // np. "Twoja odpowiedź..."
    }
}