using LogiLease.AutomationFramework.Core.DataModel.Contract;

namespace LogiLease.AutomationFramework.Core.DataModel.Answers
{
    public abstract class Question
    {
        public Role[] RolesEnabled { get; set; }
        public string QuestionText { get; set; }
        public bool IsMandatory { get; set; }
    }

    public class ComboQuestion : Question
    {
        public ComboQuestion(string questionText, Role[] roles, bool mandatory = true)
        {
            RolesEnabled = roles;
            QuestionText = questionText;
            IsMandatory = mandatory;
        }
    }

    public class DateTimeQuestion : Question
    {
        public DateTimeQuestion(string questionText, Role[] roles, bool mandatory = true)
        {
            RolesEnabled = roles;
            QuestionText = questionText;
            IsMandatory = mandatory;
        }
    }

    public class RadioQuestion : Question
    {
        public RadioQuestion(string questionText, Role[] roles, bool mandatory = true)
        {
            RolesEnabled = roles;
            QuestionText = questionText;
            IsMandatory = mandatory;
        }
    };

    public class TextQuestion : Question
    {
        public TextQuestion(string questionText, Role[] roles, bool mandatory = true)
        {
            RolesEnabled = roles;
            QuestionText = questionText;
            IsMandatory = mandatory;
        }
    };
}