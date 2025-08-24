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
            sb.AppendLine();

            int indexQ = 1;

            if (quiz.Questions != null)
            {
                foreach (var question in quiz.Questions)
                {
                    sb.AppendLine($"{indexQ}. {question.Text}");

                    switch (question)
                    {
                        case SingleChoiceQuestion scq:
                            int indexA = 65;
                            foreach (var answer in scq.Answers)
                            {
                                sb.AppendLine($"    {(char)indexA}. {answer.Text} {(answer.IsCorrect ? "(✓)" : "")}");
                                indexA++;
                            }
                            break;

                        case MatchQuestion mq:
                            sb.AppendLine("    [Dopasuj pary]");
                            foreach (var pair in mq.Pairs)
                            {
                                sb.AppendLine($"    - {pair.Label} => {pair.ImagePath}");
                            }
                            break;

                        case OpenQuestion oq:
                            sb.AppendLine("    [Otwarta odpowiedź]");
                            sb.AppendLine($"    Poprawna: {oq.CorrectAnswer}");
                            break;

                        default:
                            sb.AppendLine("    [Nieznany typ pytania]");
                            break;
                    }

                    sb.AppendLine();
                    indexQ++;
                }
            }

            return sb.ToString();
        }
    }
}