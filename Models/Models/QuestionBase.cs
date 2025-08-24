using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public abstract class QuestionBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;


        public string? PathToFile { get; set; }


        // FK do Quizu
        public int QuizId { get; set; }


        [ForeignKey(nameof(QuizId))]
        [ValidateNever]
        public Quiz? Quiz { get; set; }
    }
}
