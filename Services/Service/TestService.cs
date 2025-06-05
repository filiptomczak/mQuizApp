using Models.Models;
using Models.ViewModels;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class TestService : ITestService
    {
        private readonly IQuizService _quizService;
        private readonly IBaseService<TestResult> _testResultService;
        private readonly IQuestionService _questionService;
        public TestService(IQuizService quizService,
            IBaseService<TestResult> testResultService,
            IQuestionService questionService)
        {
            _quizService = quizService;
            _testResultService = testResultService;
            _questionService = questionService;
        }
        public Task<TakeTestVM> CreateTest(int quizId)
        {
            var quiz = _quizService.GetQuizWithQuestionsAndAnswers(quizId);
            if (quiz == null)
            {
                return null;
            }

            var takeTestVM = new TakeTestVM
            {
                QuizId = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Questions = quiz.Questions.Select(q => new TakeQuestionVM
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    ImgPath = q.PathToFile,
                    Answers = q.Answers.Select(a => a.Text).ToList()
                }).ToList()
            };
            return Task.FromResult(takeTestVM);
        }

        public void SaveResult(TestSubmissionVM model)
        {
            var points = CheckAnswers(model.Answers);

            var testResult = new TestResult()
            {
                UserName = model.UserName,
                QuizId = model.QuizId,
                Points = points
            };

            _testResultService.AddAsync(testResult);
        }

        private int CheckAnswers(List<SubmittedAnswerVM> answers)
        {
            var result = 0;
            foreach (var answer in answers)
            {
                var questionId = answer.QuestionId;
                var correctAnswerText = _questionService
                        .Get(q => q.Id == questionId, includeProperties: "Answers")?
                        .Answers.SingleOrDefault(a => a.IsCorrect)?
                        .Text;

                if (correctAnswerText == answer.SelectedAnswer)
                    result++;
            }
            return result;
        }
    }
}
