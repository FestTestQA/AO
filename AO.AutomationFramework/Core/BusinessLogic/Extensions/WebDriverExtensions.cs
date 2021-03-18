using AO.AutomationFramework.Core.BusinessLogic.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AO.AutomationFramework.Core.BusinessLogic.Extensions
{
    public static class WebDriverExtensions
    {
        private static IWebDriver GetWebDriver(this IWebElement webElement) => ((IWrapsDriver)webElement).WrappedDriver;

        public static IWebElement TryGetElement(this IWebDriver _, Func<IWebElement> elementGetter, double timeout = 5000)
        {
            var task = Task.Factory.StartNew(() =>
            {
                IWebElement element = null;
                do
                {
                    try
                    {
                        element = elementGetter();
                    }
                    catch { element = null; }
                    Thread.Sleep(500);
                } while (element == null);
                return element;
            }, TaskCreationOptions.LongRunning);
            return task.Wait(TimeSpan.FromMilliseconds(timeout)) ? task.Result : null;
        }

        public static IReadOnlyCollection<IWebElement> TryFindWebElements(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElements(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public static IReadOnlyCollection<IWebElement> TryFindChildElements(this IWebElement parent, By by)
        {
            return parent.FindElements(by);
        }

        public static IWebElement FindElementWithDelay(this IWebDriver driver, By by)
        {
            return WaitHelper.WaitResult(() => driver.FindElement(by));
        }

        public static IWebElement FindElementWithDelay(this IWebElement root, By by)
        {
            return WaitHelper.WaitResult(() => root.FindElement(by));
        }

        public static IReadOnlyCollection<IWebElement> FindElementsByAttributeStartsWith(this IWebElement element, string tag, string attribute, string attributeValueStart)
        {
            return element.TryFindChildElements(By.CssSelector(StringHelper.GenerateCCSByAttributeStartsWith(tag, attribute, attributeValueStart)));
        }

        public static IReadOnlyCollection<IWebElement> FindElementsByAttributeStartsWith(this IWebDriver driver, string tag, string attribute, string attributeValueStart)
        {
            return driver.TryFindWebElements(By.CssSelector(StringHelper.GenerateCCSByAttributeStartsWith(tag, attribute, attributeValueStart)));
        }

        public static IReadOnlyCollection<IWebElement> FindElementsByAttributeContains(this IWebElement element, string tag, string attribute, string attributeValueContains)
        {
            return element.TryFindChildElements(By.CssSelector(StringHelper.GenerateCSSByAttributeContains(tag, attribute, attributeValueContains)));
        }

        public static void SendKeysJS(this IWebElement root, string text)
        {
            var wd = root.GetWebDriver();
            ((IJavaScriptExecutor)wd).ExecuteScript(string.Format("arguments[0].value='{0}'", text), root);
            ((IJavaScriptExecutor)wd).ExecuteScript($"arguments[0].dispatchEvent(new Event('input'))", root);
        }

        public static void ClickWhenClickable(this IWebElement element)
        {
            var check = false;
            try
            {
                WaitHelper.WaitUntil(() => element.Enabled);
                element.WaitForJS();
            }
            catch (WebDriverException ex) when (ex.Message.Contains("jQuery is not defined")) { }
            while (!check)
            {
                try
                {
                    element.Click();
                    check = true;
                }
                catch (Exception) //when (ex is ElementClickInterceptedException || ex is ElementNotInteractableException)
                { }
            }
        }

        public static void WaitForJS(this IWebElement element)
        {
            WebDriverWait webDriverWait = new WebDriverWait(element.GetWebDriver(), new TimeSpan(0, 0, 10));
            webDriverWait.Until((wd =>
            ((IJavaScriptExecutor)wd).ExecuteScript("return document.readyState").Equals("complete")));
            webDriverWait.Until(wd =>
            ((IJavaScriptExecutor)wd).ExecuteScript("return jQuery.active==0").Equals(true));
        }

        public static void ScrollToView(this IWebElement element)
        {
            var js = $"arguments[0].scrollIntoView({{ behavior: 'auto', block: 'center' }});";
            ((IJavaScriptExecutor)element.GetWebDriver()).ExecuteScript(js, element);
            element.ScrollToElement();
        }

        public static void ScrollToElement(this IWebElement element)
        {
            ((IJavaScriptExecutor)element.GetWebDriver()).ExecuteScript($"arguments[0].scrollIntoView(true);", element);
        }

        public static void JSClick(this IWebElement element)
        {
            ((IJavaScriptExecutor)element.GetWebDriver()).ExecuteScript("arguments[0].click();", element);
        }

        public static bool IsElementInViewPort(this IWebElement el)
        {
            var jse = (IJavaScriptExecutor)el.GetWebDriver();
            return (bool)jse.ExecuteScript("var elem = arguments[0],                   "
                                           + "  box = elem.getBoundingClientRect(),    "
                                           + "  cx = box.left + box.width / 2,         "
                                           + "  cy = box.top + box.height / 2,         "
                                           + "  e = document.elementFromPoint(cx, cy); "
                                           + "for (; e; e = e.parentElement) {         "
                                           + "  if (e === elem)                        "
                                           + "    return true;                         "
                                           + "}                                        "
                                           + "return false;                            ", el);
        }
    }
}