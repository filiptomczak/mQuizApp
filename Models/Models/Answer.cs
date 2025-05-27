using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public bool IsCorrect { get; set; }=false;
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        [ValidateNever]
        public Question? Question { get; set; }
    }
}
