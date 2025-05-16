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
        public IFormFile? UploadedFile { get; set; }  // tylko na potrzeby przesy³ania
        public List<Answer> Answers { get; set; } = new List<Answer>();

    }
}
