using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class MatchPair
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ImagePath { get; set; } = string.Empty; // lewa strona
        [Required]
        public string Label { get; set; } = string.Empty; // prawa strona
        public int QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        [ValidateNever]
        public MatchQuestion? Question { get; set; }
    }
}
