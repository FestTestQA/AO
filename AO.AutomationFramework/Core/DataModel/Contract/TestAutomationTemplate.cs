using LogiLease.AutomationFramework.Core.DataModel.Answers;
using System.Collections.Generic;
using System.Linq;

namespace LogiLease.AutomationFramework.Core.DataModel.Contract
{
    public class TestAutomationTemplate : IContractType
    {
        private readonly Role[] roles = new Role[]{
                new Role("Assignee"),
                new Role("Landlord"),
                new Role("Tenant"),
                new Role("Undertenant") };

        public string Type { get => "Test Automation Template"; }

        public List<Question> Questionnaire
        {
            get
            {
                return new List<Question>() {
                new TextQuestion(
                    "no default | optional",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "no default | mandatory1",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray()),
                new TextQuestion(
                    "no default | mandatory2",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray()),
                new TextQuestion(
                    "default | optional",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "Land Registry Search",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "no default | optional | approval1",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "no default | mandatory | approval1",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray()),
                new TextQuestion(
                    "default | optional | approval1",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "All can edit",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "All can view assignee can edit1",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "None can see Assignee can edit",
                    Roles.Where(r=> r.RoleName == "Assignee").ToArray(), false),
                new TextQuestion(
                    "None can see Assignee can edit2",
                    Roles.Where(r=> r.RoleName == "Assignee").ToArray(), false),
                new TextQuestion(
                    "None can see Assignee can edit3",
                    Roles.Where(r=> r.RoleName == "Assignee").ToArray(), false),
                new TextQuestion(
                    "Assignee and Tenant can edit and approve",
                    Roles.Where(r=> r.RoleName == "Assignee" || r.RoleName == "Tenant" ).ToArray(), false),
                new TextQuestion(
                    "Assignee edit and approve Tenant can approve",
                    Roles.Where(r=> r.RoleName == "Assignee" || r.RoleName == "Tenant" ).ToArray(), false),
                new TextQuestion(
                    "Text",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "Number",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new DateTimeQuestion(
                    "Date",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "Email",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false),
                new TextQuestion(
                    "Phone",
                    Roles.Where(r=> r.RoleName == "Assignee" ||r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "UnderTenant" ).ToArray(), false)
            };
            }
        }

        public Role[] Roles => roles;

        public Property Property { get; set; }

        public TestAutomationTemplate(Property property)
        {
            Property = property;
            TestDataStorage.TestDataStorage.Instance.ContractType = this;
        }
    }
}