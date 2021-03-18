using LogiLease.AutomationFramework.Core.DataModel.Answers;
using System.Collections.Generic;
using System.Linq;

namespace LogiLease.AutomationFramework.Core.DataModel.Contract
{
    public class HeadLease : IContractType
    {
        private readonly Role[] roles = new Role[]{
                new Role("Landlord"),
                new Role("Tenant"),
                new Role("Tenant's Guarantor"),
                new Role("Assignee"),
                new Role("Assignee's Guarantor")};

        public string Type { get => "License to Assign HeadLease"; }

        public List<Question> Questionnaire
        {
            get
            {
                return new List<Question>() {
                new RadioQuestion(
                    "Is the use of the Premises being changed on completion of the Assignment?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new RadioQuestion(
                    "Is the change of use personal to the assignee?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new TextQuestion(
                    "What is the New Use?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new ComboQuestion(
                    "Which Use Class does this use fall in to?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new ComboQuestion(
                    "Further Changes permitted",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new ComboQuestion(
                    "Further changes will be",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new RadioQuestion(
                    "Is the outgoing Tenant providing an Authorised Guarantee Agreement?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new ComboQuestion(
                    "For how long will the Licence be valid?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new RadioQuestion(
                    "Is a premium payable for the assignment of the lease?",
                    Roles.Where(r=> r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new TextQuestion(
                    "How much is the premium?",
                    Roles.Where(r=>r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new RadioQuestion(
                    "Will there be a Rent Deposit?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Assignee" ).ToArray()),
                new TextQuestion(
                    "How much will the Rent Deposit be?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Assignee" ).ToArray()),
                new TextQuestion(
                    "Will there be a Rent Deposit?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Assignee" ).ToArray()),
                new RadioQuestion(
                    "Is VAT charged on the rent?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Assignee" ).ToArray()),
                new RadioQuestion(
                    "Is the Lease already registered at Land Registry?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new TextQuestion(
                    "Land Registry title number",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new RadioQuestion(
                    "Are there more than 7 years of the lease term remaining?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new DateTimeQuestion(
                    "What is the date of the lease?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new TextQuestion(
                    "Who was the original Landlord?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new TextQuestion(
                    "Who was the original Tenant?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray()),
                new TextQuestion(
                    "Who was the original Tenant's Guarantor?",
                    Roles.Where(r=> r.RoleName == "Landlord" || r.RoleName == "Tenant" || r.RoleName == "Assignee" ).ToArray(), false),
                };
            }
        }

        public Role[] Roles => roles;

        public Property Property { get; set; }

        public HeadLease(Property property)
        {
            Property = property;
            TestDataStorage.TestDataStorage.Instance.ContractType = this;
        }
    }
}