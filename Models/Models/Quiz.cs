using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Question>Questions { get; set; }
    }
}
