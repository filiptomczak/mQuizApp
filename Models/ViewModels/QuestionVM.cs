using Microsoft.AspNetCore.Http;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class QuestionVM
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? PathToFile { get; set; }
        public int QuizId { get; set; }
        public IFormFile? UploadedFile { get; set; }  // tylko do przesy³ania nowych plików

        public QuestionType Type { get; set; }  // zmiana na enum zamiast stringa

        // tylko dla SingleChoice
        public List<AnswerVM> Answers { get; set; } = new();

        // tylko dla Match
        public List<PairVM> Pairs { get; set; } = new();

        // tylko dla Open
        public string? CorrectAnswer { get; set; }

    }
}
