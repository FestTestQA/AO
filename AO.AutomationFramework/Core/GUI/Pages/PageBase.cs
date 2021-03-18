using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using AO.AutomationFramework.Core.BusinessLogic.Helpers;
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

        public void LogOut()
        {
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
    }
}