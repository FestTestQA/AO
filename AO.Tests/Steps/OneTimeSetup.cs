using AO.AutomationFramework.Core.BusinessLogic.Variables;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.IO;

namespace AO.Tests
{
    [SetUpFixture]
    public sealed class OneTimeSetup
    {
        private static void KillAllDrivers()
        {
            Console.WriteLine("KillAllDrivers executed");
            var drivers = Process.GetProcessesByName("chromedriver");
            foreach (var driver in drivers)
            {
                try { driver.Kill(); }
                catch (UnauthorizedAccessException) { }
            }

            drivers = Process.GetProcessesByName("geckodriver");
            foreach (var driver in drivers)
            {
                try { driver.Kill(); }
                catch (UnauthorizedAccessException) { }
            }
        }

        [OneTimeSetUp]
        public static void BeforeTestRun()
        {
            KillAllDrivers();
        }

        [OneTimeTearDown]
        public static void AfterPortalTestRun()
        {
            KillAllDrivers();
        }

        public static void BrowseHomePage(IWebDriver driver)
        {
            driver.Url = $@"{CommonVar.GetSiteURL()}";
        }
    }
}