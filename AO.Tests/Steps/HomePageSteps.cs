using AO.AutomationFramework.Core.GUI.Pages;
using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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