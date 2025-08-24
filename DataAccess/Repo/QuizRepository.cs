using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class QuizRepository : BaseRepository<Quiz>, IQuizRepository
    {
        public QuizRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<TakeTestVM?> GetTestVMWithQuestionsAndAnswersAsync(int id)
        {
            var quiz = await _context.Quizzes
        .Include(q => q.Questions)
            .ThenInclude(q => (q as SingleChoiceQuestion)!.Answers) // ładowanie odpowiedzi
        .Include(q => q.Questions)
            .ThenInclude(q => (q as MatchQuestion)!.Pairs) // ładowanie par
        .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null) return null;

            var vm = new TakeTestVM
            {
                QuizId = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Questions = quiz.Questions.Select(ques =>
                {
                    if (ques is SingleChoiceQuestion sc)
                    {
                        return new TakeQuestionVM
                        {
                            QuestionId = sc.Id,
                            Text = sc.Text,
                            ImgPath = sc.PathToFile,
                            Type = QuestionType.SingleChoice,
                            Answers = sc.Answers.Select(a => a.Text).ToList()
                        };
                    }
                    else if (ques is MatchQuestion mq)
                    {
                        return new TakeQuestionVM
                        {
                            QuestionId = mq.Id,
                            Text = mq.Text,
                            ImgPath = mq.PathToFile,
                            Type = QuestionType.Match,
                            Pairs = mq.Pairs.Select(p => new MatchPairVM
                            {
                                Id = p.Id,
                                ImagePath = p.ImagePath,
                                Label = p.Label
                            }).ToList()
                        };
                    }
                    else if (ques is OpenQuestion oq)
                    {
                        return new TakeQuestionVM
                        {
                            QuestionId = oq.Id,
                            Text = oq.Text,
                            ImgPath = oq.PathToFile,
                            Type = QuestionType.Open
                        };
                    }

                    throw new InvalidOperationException("Unknown question type");
                }).ToList()
            };

            return vm;
        }
    }
}