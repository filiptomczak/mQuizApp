using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        bool IsCorrect { get; set; }
        public int QuestionId { get;set; }
        public Question Question { get; set; }
    }
}