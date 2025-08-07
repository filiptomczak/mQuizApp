using DataAccess.IRepo;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuizService _quizService;
        private readonly IBaseService<TestResult> _testResultService;
        private readonly IQuestionService _questionService;
        public TestService(
            IUnitOfWork unitOfWork,
            IQuizService quizService,
            IBaseService<TestResult> testResultService,
            IQuestionService questionService)
        {
            _unitOfWork = unitOfWork;
            _quizService = quizService;
            _testResultService = testResultService;
            _questionService = questionService;
        }
        public async Task<TakeTestVM> CreateTest(int quizId)
        {
            var takeTestVM = await _quizService.GetTestVMWithQuestionsAndAnswersAsync(quizId);
            if (takeTestVM == null)
            {
                return null;
            }
            return takeTestVM;
        }

        public async Task SaveResult(TestSubmissionVM model)
        {
            var points = CheckAnswers(model.Answers);

            var testResult = new TestResult()
            {
                UserName = model.UserName,
                QuizId = model.QuizId,
                Points = points
            };

            await _testResultService.AddAsync(testResult);
            await _unitOfWork.CommitAsync();
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
