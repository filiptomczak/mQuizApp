using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class TestSubmissionVM
    {
        public int QuizId { get; set; }
        public string UserName { get; set; }
        public List<SubmittedAnswerVM> Answers { get; set; }
    }

    public class SubmittedAnswerVM
    {
        public int QuestionId { get; set; }
        public string SelectedAnswer { get; set; }
    }
}
