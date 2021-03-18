namespace LogiLease.AutomationFramework.Core.DataModel.Answers
{
    public class QuestionTextAnswer : AbstractQuestionAnswer
    {
        public string Answer { get; set; }

        public QuestionTextAnswer(TextQuestion question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}