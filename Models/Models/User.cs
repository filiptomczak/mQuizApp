using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }

        public ICollection<UserQuiz> UserQuizes{ get; set; }
    }
}
