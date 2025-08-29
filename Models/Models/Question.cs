using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Text { get; set; }
        public string? PathToFile{ get; set; }
        public int QuizId { get; set; }
        public float Points { get; set; } = 1.0f;
        [ForeignKey("QuizId")]
        [ValidateNever]
        public Quiz? Quiz { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}