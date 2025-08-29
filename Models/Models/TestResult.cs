using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class TestResult
    {
        public int Id {  get; set; }
        public string UserName { get; set; }
        public int QuizId {  get; set; }
        public float Points { get; set; }

    }
}
