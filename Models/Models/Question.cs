using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public QuestionType TypeOfQuestion { get; set; }
        public string? PathToFile{ get; set; }
        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }
        //public ICollection<Answer> Answers { get; set; }
    }
    public enum QuestionType
    {
        Text,
        Image,
        Sound,
    }
}