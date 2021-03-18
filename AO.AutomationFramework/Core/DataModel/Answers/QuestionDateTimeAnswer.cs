using System;

namespace LogiLease.AutomationFramework.Core.DataModel.Answers
{
    public class QuestionDateTimeAnswer : AbstractQuestionAnswer
    {
        public DateTime Answer { get; set; }

        public QuestionDateTimeAnswer(DateTimeQuestion question, DateTime answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}