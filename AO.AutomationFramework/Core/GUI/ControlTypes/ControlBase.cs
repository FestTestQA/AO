using AO.AutomationFramework.Core.BusinessLogic.Helpers;
using OpenQA.Selenium;
using System.Linq;

namespace AO.AutomationFramework.Core.GUI.ControlTypes
{
    public abstract class ControlBase
    {
        protected readonly IWebDriver Driver;

        public readonly IWebElement WebElement;

        public ControlBase(IWebDriver driver, IWebElement el)
        {
            Driver = driver;
            WebElement = el;
        }

        public ControlBase(IWebDriver webDriver, string wrapperCSS, bool useXpathInstead = false)
        {
            if (useXpathInstead)
            {
                WaitHelper.WaitResult(() => webDriver.FindElements(By.XPath(wrapperCSS)).FirstOrDefault(el => el.Displayed));
                WebElement = webDriver.FindElements(By.XPath(wrapperCSS)).FirstOrDefault(el => el.Displayed);
            }
            else
            {
                WaitHelper.WaitResult(() => webDriver.FindElements(By.CssSelector(wrapperCSS)).FirstOrDefault(el => el.Displayed));
                WebElement = webDriver.FindElements(By.CssSelector(wrapperCSS)).FirstOrDefault(el => el.Displayed);
            }
            this.Driver = webDriver;
        }
    }
}