namespace LogiLease.AutomationFramework.Core.DataModel.Answers
{
    public class QuestionComboAnswer : AbstractQuestionAnswer
    {
        public string Answer { get; set; }

        public QuestionComboAnswer(ComboQuestion question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}