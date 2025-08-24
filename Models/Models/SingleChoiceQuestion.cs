using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class SingleChoiceQuestion:QuestionBase
    {
        public List<Answer> Answers { get; set; } = new();
    }
}
