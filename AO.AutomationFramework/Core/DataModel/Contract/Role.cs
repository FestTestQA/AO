using System.Collections.Generic;

namespace LogiLease.AutomationFramework.Core.DataModel.Contract
{
    public class Role
    {
        public string RoleName { get; set; }

        public List<string> Representatives { get; set; }

        public Role(string roleName)
        {
            RoleName = roleName;
            Representatives = new List<string>();
        }
    }
}