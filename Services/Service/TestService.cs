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

        public async Task<bool> SaveResult(TestSubmissionVM model)
        {
            var points = CheckAnswers(model.Answers);

            var testResult = new TestResult()
            {
                UserName = model.UserName,
                QuizId = model.QuizId,
                Points = points
            };

            try
            {
                await _testResultService.AddAsync(testResult);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int CheckAnswers(List<SubmittedAnswerVM> answers)
        {
            var result = 0;

            foreach (var submitted in answers)
            {
                var question = _questionService.Get(q => q.Id == submitted.QuestionId);

                if (question == null)
                    continue;

                switch (question)
                {
                    case SingleChoiceQuestion sc:
                        var correct = sc.Answers.SingleOrDefault(a => a.IsCorrect)?.Text;
                        if (!string.IsNullOrEmpty(correct) && submitted.SelectedAnswer == correct)
                            result++;
                        break;

                    case OpenQuestion oq:
                        if (!string.IsNullOrEmpty(oq.CorrectAnswer) &&
                            string.Equals(oq.CorrectAnswer.Trim(), submitted.SelectedAnswer?.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            result++;
                        }
                        break;

                    case MatchQuestion mq:
                        // TODO: tutaj możesz porównać submitted.Pairs vs mq.Pairs
                        // np. sprawdzając czy wszystkie dobrane elementy są poprawne
                        break;

                    default:
                        throw new NotSupportedException($"Nieobsługiwany typ pytania: {question.GetType().Name}");
                }
            }

            return result;
        }
    }
}
