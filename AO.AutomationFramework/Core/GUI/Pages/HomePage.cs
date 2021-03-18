using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using OpenQA.Selenium;
using System.Linq;

namespace AO.AutomationFramework.Core.GUI.Pages
{
    public class HomePage : PageBase
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        public IWebElement ProductTypeCombo
        {
            get { return Driver.FindElements(By.ClassName("hmcChoiceButton")).FirstOrDefault(hmc => hmc.GetAttribute("data-filtername") == "cat"); }
        }

        private IWebElement ProductTypeFilterFlyOut
        {
            get { return Driver.FindElement(By.ClassName("filterByCategory")); }
        }

        public IWebElement ProductTypeFilterButton(string buttonText)
        {
            return ProductTypeFilterFlyOut.FindElementsByAttributeStartsWith("div", "data-tag-category", buttonText).ElementAtOrDefault(0);
        }

        public IWebElement SearchButton
        {
            get { return Driver.FindElements(By.ClassName("hmcButton")).First(el => el.Text.Contains("Search")); }
        }
    }
}