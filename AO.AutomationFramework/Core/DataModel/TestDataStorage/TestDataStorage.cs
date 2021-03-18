using LogiLease.AutomationFramework.Core.DataModel.Contract;
using System;
using System.Collections.Generic;

namespace LogiLease.AutomationFramework.Core.DataModel.TestDataStorage
{
    public class TestDataStorage
    {
        private static TestDataStorage _instance;

        private List<PortalUser> portalUsers = new List<PortalUser>();

        public static TestDataStorage Instance
        {
            get
            {
                try
                {
                    return _instance ?? (_instance = new TestDataStorage());
                }
                catch
                {
                    return null;
                }
            }

            //protected set { _instance = value; }
        }

        public List<PortalUser> PortalUsers
        {
            get { return portalUsers; }
            set { portalUsers = value; }
        }

        public IContractType ContractType { get; set; }

        private TestDataStorage()
        { }
    }
}