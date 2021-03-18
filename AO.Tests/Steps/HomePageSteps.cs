using AO.AutomationFramework.Core.GUI.Pages;
using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using OpenQA.Selenium;

namespace AO.Tests.Steps
{
    internal class HomePageSteps : BaseSteps
    {
        private readonly HomePage homePage;

        internal HomePageSteps(string username, IWebDriver driver) : base(username, driver)
        {
            homePage = new HomePage(driver);
        }

        internal void WhenIHaveClickedProductTypeCombo()
        {
            homePage.ProductTypeCombo.Click();
        }

        internal void WhenIHaveSelectedProductType(string productType)
        {
            homePage.ProductTypeFilterButton(productType).Click();
        }

        internal void WhenIHaveClickedSearchButton()
        {
            homePage.SearchButton.ClickWhenClickable();
        }
    }
}