using NUnit.Framework.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AO.Tests.DataModel.TestData
{
    internal class TestDataStorage
    {
        private static readonly ConcurrentBag<(TestDataStorage Instance, string PID)> _instance = new ConcurrentBag<(TestDataStorage instance, string PID)>();

        private TestDataStorage()
        { }

        internal static TestDataStorage Instance
        {
            get
            {
                if (!TestExecutionContext.CurrentContext.CurrentTest.Properties.Keys.Contains("PID"))
                {
                    TestExecutionContext.CurrentContext.CurrentTest.Properties.Add("PID",
                        Thread.CurrentThread.ManagedThreadId +
                        TestExecutionContext.CurrentContext.CurrentTest.FullName +
                        new Random().Next(0, 999).ToString());
                }
                var pid = TestExecutionContext.CurrentContext.CurrentTest.Properties.Get("PID").ToString();
                if (!_instance.Any(i => i.PID == pid))
                {
                    _instance.Add((Instance: new TestDataStorage(), PID: pid));
                }

                try
                {
                    return _instance.FirstOrDefault(i => i.PID == pid).Instance;
                }
                catch
                {
                    return null;
                }
            }
            set { }
        }

        //require this for cases when subsequent test needs previous ordered tests contract data.
        internal static TestDataStorage LastTest { get; private set; } = new TestDataStorage();

        public List<PortalUser> PortalUsers { get; set; } = new List<PortalUser>();
    }
}