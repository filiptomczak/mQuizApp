using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class QuizVM
    {
        public Quiz Quiz { get; set; }
        public List<QuestionVM> Questions { get; set; } //= new List<Question>();
    }
}
