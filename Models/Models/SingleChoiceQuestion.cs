namespace Models.Models
{
    public class SingleChoiceQuestion : QuestionBase
    {
        public List<Answer> Answers { get; set; } = new();
    }
}
