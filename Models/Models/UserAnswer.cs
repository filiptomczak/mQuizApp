using System.ComponentModel.DataAnnotations;

namespace Models.Models
{ 
    public class UserAnswer
    {
        [Key]
        public int Id { get; set; }

        public int UserQuizId { get; set; }
        public UserQuiz UserQuiz{ get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int AnswerId { get; set; }
        public Answer Answer{ get; set; }
    }
}