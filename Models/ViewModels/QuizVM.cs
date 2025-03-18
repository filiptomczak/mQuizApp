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
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
