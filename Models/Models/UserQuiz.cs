using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class UserQuiz
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateMade { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}