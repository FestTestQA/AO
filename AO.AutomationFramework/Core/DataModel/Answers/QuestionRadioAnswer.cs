using System;

namespace LogiLease.AutomationFramework.Core.DataModel.Answers
{
    public class QuestionRadioAnswer : AbstractQuestionAnswer
    {
        public string Answer { get; set; }

        public QuestionRadioAnswer(RadioQuestion question, string answer)
        {
            Question = question;
            Answer = answer; ;
        }
    }
}