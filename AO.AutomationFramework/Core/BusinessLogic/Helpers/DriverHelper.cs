using AO.AutomationFramework.Core.BusinessLogic.Variables;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AO.AutomationFramework.Core.BusinessLogic.Helpers
{
    public static class DriverHelper
    {
        public static List<(string Username, IWebDriver Driver, string Pid)> ActiveDrivers { get; } = new List<(string Username, IWebDriver Driver, string Pid)>();

        private static IWebDriver InitDriver<TWebDriver>((DriverOptions driverOptions, Size size) driverOptions, string url) where TWebDriver : IWebDriver
        {
            Type type = typeof(TWebDriver);
            IWebDriver _newDriver;
            switch (type)
            {
                case Type _ when type == typeof(ChromeDriver):
                    if (driverOptions.driverOptions != null)
                    {
                        _newDriver = new ChromeDriver(CommonVar.ResourcesFolderPath, (ChromeOptions)driverOptions.driverOptions);
                    }
                    else
                    {
                        _newDriver = new ChromeDriver(CommonVar.ResourcesFolderPath);
                    }
                    break;

                case Type _ when type == typeof(InternetExplorerDriver):
                    _newDriver = new InternetExplorerDriver(CommonVar.ResourcesFolderPath, new InternetExplorerOptions
                    {
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true
                    });
                    break;

                case Type _ when type == typeof(FirefoxDriver):
                    if (driverOptions.driverOptions != null)
                    {
                        _newDriver = new FirefoxDriver(CommonVar.ResourcesFolderPath, (FirefoxOptions)driverOptions.driverOptions);
                        if (!driverOptions.size.IsEmpty)
                        {
                            _newDriver.Manage().Window.Size = driverOptions.size;
                        }
                    }
                    else
                    {
                        _newDriver = new FirefoxDriver(CommonVar.ResourcesFolderPath);
                    }

                    break;

                case Type _ when type == typeof(OperaDriver):
                    if (driverOptions.driverOptions != null)
                    {
                        _newDriver = new OperaDriver(CommonVar.ResourcesFolderPath, (OperaOptions)driverOptions.driverOptions);
                    }
                    else
                    {
                        _newDriver = new OperaDriver(CommonVar.ResourcesFolderPath, new OperaOptions() { BinaryLocation = CommonVar.ResourcesFolderPath });
                    }
                    break;

                case Type _ when type == typeof(EdgeDriver):
                    _newDriver = new EdgeDriver(CommonVar.ResourcesFolderPath);
                    break;

                default:
                    throw new Exception("Specified browser is not supported.\nhttps://www.seleniumhq.org/download/");
            }
            _newDriver.Navigate().GoToUrl(url);
            return _newDriver;
        }

        internal static void DivideDeskTop(string pid)
        {
            if (ActiveDrivers.Where(d => d.Pid == pid).Count() > 1)
            {
                var fullDesktop = Screen.AllScreens.Select(screen => screen.Bounds)
                                                   .Aggregate(Rectangle.Union).Size;
                Size size = new Size(Convert.ToInt32(fullDesktop.Width / ActiveDrivers.Where(d => d.Pid == pid).Count()), Convert.ToInt32(fullDesktop.Height));
                Point rightMost = new Point(0, 0);
                foreach (var (Username, Driver, Pid) in ActiveDrivers.Where(d => d.Pid == pid))
                {
                    Driver.Manage().Window.Position = rightMost;
                    Driver.Manage().Window.Size = size;
                    rightMost.X += size.Width;
                }
            }
        }

        public static (string Username, IWebDriver Driver, string Pid) GetUsersDriver<TWebDriver>(string username, DriverOptions driverOption = null, bool divideDeskTop = true) where TWebDriver : IWebDriver
        {
            var pid = TestExecutionContext.CurrentContext.CurrentTest.Properties.ContainsKey("PID")
                ? TestExecutionContext.CurrentContext.CurrentTest.Properties.Get("PID").ToString()
                : string.Empty;
            if (!ActiveDrivers.Any(t => t.Username == username && t.Pid == pid))
            {
                ActiveDrivers.Add((Username: username, Driver: InitDriver<TWebDriver>((driverOption, new Size()), CommonVar.GetSiteURL()), pid));
                ActiveDrivers.First(t => t.Username == username && t.Pid == pid).Driver.Manage().Window.Maximize();
                if (divideDeskTop)
                {
                    DivideDeskTop(pid);
                }
            }
            return ActiveDrivers.First(t => t.Username == username && t.Pid == pid);
        }

        public static List<(string Username, IWebDriver Driver, string Pid)> GetUsersDriverAsync<TWebDriver>(List<string> usernames, DriverOptions driverOption = null) where TWebDriver : IWebDriver
        {
            return usernames.Select(u => GetUsersDriver<TWebDriver>(u, driverOption, false)).ToList();
        }

        public static IEnumerable<(string Username, IWebDriver Driver, string Pid)> GetAllDriversByPid()
        {
            return ActiveDrivers.Where(t => t.Pid == TestExecutionContext.CurrentContext.CurrentTest.Properties.Get("PID").ToString()).ToList();
        }

        public static void QuitAllDriversByPid()
        {
            foreach ((string Username, IWebDriver Driver, string Pid) in ActiveDrivers)
            {
                if (!TestExecutionContext.CurrentContext.CurrentTest.Properties.ContainsKey("PID") || Pid == TestExecutionContext.CurrentContext.CurrentTest.Properties.Get("PID").ToString())
                {
                    Driver.Quit();
                    Console.WriteLine($"QuitAllDriversByPid: {Pid} executed for Username: {Username}");
                }
            }
            //TODO: to update if needed. Changed for parallel execution
            //ActiveDrivers.Where(d => d.Pid == TestExecutionContext.CurrentContext.CurrentTest.Properties.Get("PID").ToString()) ;
        }

        public static void QuitAllDrivers()
        {
            foreach ((string Username, IWebDriver Driver, string _) in ActiveDrivers)
            {
                Driver.Quit();
                Console.WriteLine($"QuitAllDriversByPid: Executed for Username: {Username}");
            }
        }
    }
}