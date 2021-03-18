namespace LogiLease.AutomationFramework.Core.DataModel.Answers
{
    public class QuestionAnswer
    {
        public Question Question { get; set; }
        public string Answer { get; set; }

        public QuestionAnswer(Question question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}