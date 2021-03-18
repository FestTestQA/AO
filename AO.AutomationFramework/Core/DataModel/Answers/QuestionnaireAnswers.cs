using System.Collections.Generic;

namespace LogiLease.AutomationFramework.Core.DataModel.Answers
{
    public class QuestionnaireAnswers
    {
        public List<QuestionAnswer> List { get; set; }

        public QuestionnaireAnswers(List<QuestionAnswer> list)
        {
            List = list;
        }
    }
}