using System;

namespace LogiLease.AutomationFramework.Core.DataModel.TestDataStorage
{
    [System.AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestDataManager : Attribute
    {
        public TestDataManager(TestDataTypes type, string[] args)
        {
            switch (type)
            {
                case TestDataTypes.PortalUser:
                    TestDataStorage.Instance.PortalUsers.Add(new PortalUser(args[0]));
                    break;

                case TestDataTypes.Property:
                    //TODO: code block
                    break;

                case TestDataTypes.Unit:
                    //TODO: code block
                    break;

                default:
                    //TODO: code block
                    break;
            }
        }
    }
}