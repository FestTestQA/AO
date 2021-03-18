using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using AO.AutomationFramework.Core.BusinessLogic.Helpers;
using AO.Tests.DataModel.TestData;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace AO.Tests
{
    internal sealed class BaseTest
    {
        private static void CreateTestProperties()
        {
            if (TestExecutionContext.CurrentContext.CurrentTest.Properties.Keys.Contains("PID"))
            {
                TestExecutionContext.CurrentContext.CurrentTest.Properties.Set("PID", GeneratePID());
            }
            else
            {
                TestExecutionContext.CurrentContext.CurrentTest.Properties.Add("PID", GeneratePID());
            }

            if (!TestExecutionContext.CurrentContext.CurrentTest.Properties.Keys.Contains("RetryCount"))
            {
                TestExecutionContext.CurrentContext.CurrentTest.Properties.Add("RetryCount", 0);
            }
        }

        private static string GeneratePID()
        {
            return Thread.CurrentThread.ManagedThreadId + TestExecutionContext.CurrentContext.CurrentTest.FullName + new Random().Next(0, 99999).ToString();
        }

        public static void AfterScenario(bool keepTestData = false)
        {
            if (TestExecutionContext.CurrentContext.CurrentTest.Properties.Keys.Contains("RetryCount"))
            {
                TestExecutionContext.CurrentContext.CurrentTest.Properties.Set("RetryCount",
                    int.Parse(TestExecutionContext.CurrentContext.CurrentTest.Properties.Get("RetryCount").ToString()) + 1);
            }
            if (TestExecutionContext.CurrentContext.CurrentResult.FailCount > 0)
            {
                GrabScreen();
                Console.WriteLine($"Error message: {TestExecutionContext.CurrentContext.CurrentResult.Message}");
                Console.WriteLine($"Trace: {TestExecutionContext.CurrentContext.CurrentResult.StackTrace}");
            }
            Console.WriteLine($"Repeat: {TestExecutionContext.CurrentContext.CurrentTest.Properties.Get("RetryCount")} / {TestExecutionContext.CurrentContext.CurrentRepeatCount}");
            CloseWord();
            DriverHelper.QuitAllDriversByPid();
            if (TestExecutionContext.CurrentContext.CurrentTest.GetCustomAttributes<ReuseTestData>(false).Count() > 0 && TestExecutionContext.CurrentContext.CurrentTest.GetCustomAttributes<ReuseTestData>(false)[0].KeepCurrentTestData)
            {
                TestDataStorage.Instance.CopyProperties(TestDataStorage.LastTest);
            }
        }

        public static void BeforePortalScenario<TWebDriver>() where TWebDriver : IWebDriver
        {
            CreateTestProperties();
            Console.WriteLine("New Thread Started:" + TestExecutionContext.CurrentContext.CurrentTest.Properties.Get("PID").ToString());

            if (TestExecutionContext.CurrentContext.CurrentTest.GetCustomAttributes<ReuseTestData>(false).Count() > 0 && TestExecutionContext.CurrentContext.CurrentTest.GetCustomAttributes<ReuseTestData>(false)[0].ReuseLastTestData)
            {
                TestDataStorage.LastTest.CopyProperties(TestDataStorage.Instance);
            }
        }

        public static void CloseWord()
        {
            var word = Process.GetProcessesByName("WINWORD").FirstOrDefault();
            if (word != null)
            {
                word.Kill();
            }
        }

        public static void GrabScreen()
        {
            foreach (var (Username, Driver, _) in DriverHelper.GetAllDriversByPid())
            {
                var _screenShotPath = ScreenshotHelper.GetFileName(TestExecutionContext.CurrentContext.CurrentTest.FullName + "_" + Username.Replace("@", ""));
                WaitHelper.WaitUntil(() =>
                {
                    try
                    {
                        ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(_screenShotPath);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                });
                Console.WriteLine("Screenshot saved at: " + _screenShotPath + " for PID: ");
                _screenShotPath = ScreenshotHelper.GetFileName($"{TestExecutionContext.CurrentContext.CurrentTest.Properties.Keys.FirstOrDefault(k => k == "AllureTag")}_{TestExecutionContext.CurrentContext.CurrentTest.FullName}_{Username.Replace("@", "")}");
                ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(_screenShotPath);
            }
        }
    }
}