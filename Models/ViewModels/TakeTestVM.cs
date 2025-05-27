using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class TakeTestVM
    {
        public int QuizId { get; set; }
        public string Title {  get; set; }
        public string Description { get; set; }
        public List<TakeQuestionVM> Questions { get; set; } //= new List<Question>();
    }
}
