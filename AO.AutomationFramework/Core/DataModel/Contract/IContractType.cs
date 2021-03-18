using LogiLease.AutomationFramework.Core.DataModel.Answers;
using System.Collections.Generic;

namespace LogiLease.AutomationFramework.Core.DataModel.Contract
{
    public interface IContractType
    {
        public string Type { get; }

        public Property Property { get; set; }

        public List<Question> Questionnaire { get; }

        public Role[] Roles { get; }
    }
}