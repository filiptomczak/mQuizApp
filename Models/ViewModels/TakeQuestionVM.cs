namespace Models.ViewModels
{
    public class TakeQuestionVM
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string ImgPath { get; set; }
        public List<string> Answers { get; set; }
    }
}