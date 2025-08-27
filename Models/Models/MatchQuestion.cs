namespace Models.Models
{
    public class MatchQuestion : QuestionBase
    {
        public List<MatchPair> Pairs { get; set; } = new();
    }
}
