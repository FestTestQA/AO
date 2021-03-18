using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using AO.AutomationFramework.Core.BusinessLogic.Helpers;
using AO.AutomationFramework.Core.GUI.ControlTypes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AO.AutomationFramework.Core.GUI.Pages
{
    public class PageBase
    {
        protected IWebDriver Driver;

        private IWebElement LoadingMask
        {
            get { return Driver.FindElement(By.CssSelector(".k-loading-mask")); }
        }

        public virtual IWebElement Title
        {
            get { return Driver.FindElementWithDelay(By.TagName("h1")); }
        }

        public bool IsLoadingMaskDisplayed
        {
            get
            {
                try
                {
                    bool displayed = LoadingMask.Displayed;
                    return displayed;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public IWebElement CancelButton
        {
            get { return Driver.FindElement(By.Id("btn-cancel-property")); }
        }

        public IWebElement SaveButton
        {
            get { return Driver.FindElement(By.Id("btn-save-property")); }
        }

        public PageBase(IWebDriver driver)
        {
            Driver = driver;
        }

        protected IWebElement FindElementByName(string name)
        {
            return Driver.FindElementWithDelay(By.Name(name));
        }

        protected IWebElement FindElementById(string id)
        {
            return Driver.FindElementWithDelay(By.Id(id));
        }

        protected IWebElement FindElementByXpath(string expression, IWebElement parent = null)
        {
            if (parent != null)
            {
                return parent.FindElementWithDelay(By.XPath(expression));
            }

            return Driver.FindElementWithDelay(By.XPath(expression));
        }

        protected IWebElement GetCheckBoxFromTable(IWebElement table, string nameToSelect)
        {
            var list = table.FindElements(By.TagName("tr"));
            foreach (var row in list)
            {
                var itemsInRow = row.FindElements(By.TagName("td"));
                if (itemsInRow[1].Text == nameToSelect)
                {
                    return itemsInRow[0].FindElement(By.TagName("input"));
                }
            }
            return null;
        }

        public IWebElement GetLabelByText(string text)
        {
            return Driver.FindElement(By.XPath(string.Format("//*[text()=\"{0}\"]", text)));
        }

        public IWebElement GetLabelByTextContains(string text)
        {
            return Driver.FindElement(By.XPath(string.Format("//*[contains(text(),\"{0}\")]", text)));
        }

        public IWebElement TryFindWebElement(Func<IWebElement> elementGetter)
        {
            try
            {
                return elementGetter();
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public void GoTo(string link)
        {
            Driver.Navigate().GoToUrl(link);
        }

        public void SelectFromComobobox(IWebElement combobox, string textToSelect)
        {
            var table = new SelectElement(combobox);
            table.SelectByText(textToSelect);
        }

        public void LogOut()
        {
            Driver.TryGetElement(() => Driver.FindElement(By.Id("stage_name_menu")), 5000).Click();
            Driver.TryGetElement(() => Driver.FindElement(By.XPath("//*[text()='Logout']")), 5000).Click();
        }

        public void OpenAccountSettings()
        {
            Driver.TryGetElement(() => Driver.FindElementWithDelay(By.Id("stage_name_menu")), 5000).Click();
            Driver.TryGetElement(() => Driver.FindElement(By.XPath("//*[text()='Account settings']")), 5000).Click();
        }

        public bool MessageExists(string message)
        {
            return WaitHelper.WaitUntil(() => Driver.PageSource.Contains(message));
        }

        public bool MessageDisplayed(string message)
        {
            try
            {
                Driver.FindElement(By.XPath("//*[contains(text(),'" + message + "')]"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AlertExists(string message)
        {
            return WaitHelper.WaitUntil(() => Driver.SwitchTo().Alert().Text.Contains(message));
        }

        public void ConfirmAlert()
        {
            Driver.SwitchTo().Alert().Accept();
        }

        public bool AlertExists()
        {
            try
            {
                Driver.SwitchTo().Alert();
                return true;
            }
            catch (Exception) { return false; }
        }

        public void ScrollDown()
        {
            var jse = (IJavaScriptExecutor)Driver;
            jse.ExecuteScript(string.Format("scroll(0, document.body.scrollHeight)"));
        }

        public void ScrollDown(int value)
        {
            var jse = (IJavaScriptExecutor)Driver;
            jse.ExecuteScript(string.Format("scroll(0, " + value + ")"));
        }

        public void ScrollUp(int value)
        {
            var jse = (IJavaScriptExecutor)Driver;
            jse.ExecuteScript(string.Format("scroll(" + value + ",0)"));
        }

        public void ScrollToView(IWebElement element)
        {
            var jse = (IJavaScriptExecutor)Driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public IWebDriver GetDriver()
        {
            return Driver;
        }

        //public void RestartBrowser()
        //{
        //    Driver.Quit();
        //    switch (CommonVar.browser.ToString().ToLower())
        //    {
        //        case "chrome":
        //            Driver = new ChromeDriver(Path.GetFullPath(@"Tools"));
        //            break;
        //        case "ie":
        //            Driver = new InternetExplorerDriver(@"Tools", new InternetExplorerOptions
        //            {
        //                IntroduceInstabilityByIgnoringProtectedModeSettings = true
        //            });
        //            break;
        //        case "safari":
        //            Driver = new SafariDriver();
        //            break;
        //        case "firefox":
        //            Driver = new FirefoxDriver();
        //            break;
        //        default:
        //            throw new Exception("Specified browser is not supported.");
        //    }
        //    PageHelper.Initialize(Driver);
        //    Driver.Manage().Window.Maximize();
        //}

        public void JavaScriptClick(IWebElement element)
        {
            var jse = (IJavaScriptExecutor)Driver;
            jse.ExecuteScript("arguments[0].click();", element);
        }

        public void JavaScripMouseOver(IWebElement element)
        {
            const string script = "var evt = document.createEvent('MouseEvents');" +
                                  "evt.initMouseEvent('mouseover',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);" +
                                  "arguments[0].dispatchEvent(evt);";
            var jse = (IJavaScriptExecutor)Driver;
            jse.ExecuteScript(script, element);
        }

        #region SideBar

        public IWebElement SideBarToggle
        {
            get { return Driver.FindElement(By.CssSelector(".main-nav-toggle")); }
        }

        public IWebElement HomeButton
        {
            get { return Driver.FindElement(By.CssSelector(".nav > li:nth-child(1) > a:nth-child(2)")); }
        }

        public IWebElement UserName
        {
            get { return Driver.FindElement(By.CssSelector("#logoutForm > a:nth-child(2)")); }
        }

        public object PageWebElement { get; private set; }

        #endregion SideBar

        private IWebElement NotificationsContainer
        {
            get
            {
                return Driver.FindElementsByAttributeStartsWith("div", "class", "notices is-bottom").ElementAt(0);
            }
        }

        public List<(IWebElement Toast, string Body, IWebElement Link, IWebElement Close)> Notifications
        {
            get
            {
                List<(IWebElement Toast, string Body, IWebElement Link, IWebElement Close)> notificationList = new List<(IWebElement Toast, string Body, IWebElement Link, IWebElement Close)>();
                List<IWebElement> els = new List<IWebElement>();
                try
                {
                    els = NotificationsContainer.FindElementsByAttributeContains("div", "role", "alert").ToList();
                }
                catch { }
                foreach (var notification in els)
                {
                    var body = notification.FindElement(By.ClassName("body")).Text;
                    var links = notification.FindElementsByAttributeStartsWith("a", "class", "action-link");
                    var link = links.Count > 0 ? Driver.TryGetElement(() => links.ElementAt(0)) : null;
                    var close = notification.FindElement(By.ClassName("close-toast"));
                    notificationList.Add((notification, body, link, close));
                }
                return notificationList;
            }
        }
    }
}