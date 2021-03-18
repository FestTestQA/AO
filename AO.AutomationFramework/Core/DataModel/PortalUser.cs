using LogiLease.AutomationFramework.Core.DataModel.TestDataStorage;
using System.Collections.Generic;
using System.Linq;

namespace LogiLease.AutomationFramework.Core.DataModel
{
    public class PortalUser : ITestData
    {
        public string UserName { get; }

        public string PassWord
        {
            get
            {
                return GetUserPassword(UserName);
            }
        }

        public PortalUser(string name)
        {
            UserName = name;
        }

        private string GetUserPassword(string userName)
        {
            Dictionary<string, string> userList = new Dictionary<string, string>
            {
                { "royeje6596@hubopss.com", "Testing123!" },//is admin
                { "pamoc92932@2go-mail.com", "Testing123!" },
                { "kowora7167@lagsixtome.com", "Testing123!" },
                { "mevav12177@lagsixtome.com", "Testing123!" },
                { "joxeloc373@officemalaga.com", "Testing123!" },
                { "bitosa3485@hubopss.com", "Testing123!" }
            };
            return userList.First(u => u.Key == userName).Value;
        }
    }
}