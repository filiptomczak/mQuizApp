using Models.Models;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class QuizExportService : IQuizExportService
    {
        private readonly IQuizService _quizService;
        public QuizExportService(IQuizService quizService)
        {
            _quizService = quizService;
        }
        public string ExportQuizAsText(Quiz quiz)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Quiz: {quiz.Title}");
            sb.AppendLine($"Opis: {quiz.Description}");

            int indexQ = 1;

            if (quiz.Questions != null)
            {
                foreach (var question in quiz.Questions)
                {
                    int indexA = 65;
                    sb.AppendLine($"{indexQ}. {question.Text}");
                    foreach (var answer in question.Answers)
                    {
                        sb.AppendLine($"    {(char)(indexA)}. {answer.Text} {(answer.IsCorrect ? "(✓)" : "")}");
                        indexA++;
                    }
                    sb.AppendLine();
                    indexQ++;
                }
            }
            return sb.ToString();
        }
    }
}
