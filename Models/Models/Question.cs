using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public string PathToFile{ get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
    public enum QuestionType
    {
        Text,
        Image,
        Sound,
    }
}