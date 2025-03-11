using System.ComponentModel.DataAnnotations;

namespace Models.Models
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
