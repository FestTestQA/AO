using AO.AutomationFramework.Core.BusinessLogic.Helpers;
using AO.AutomationFramework.Core.BusinessLogic.Variables;
using AO.Tests.DataModel;
using AO.Tests.Steps;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;

namespace AO.Tests
{
    [TestFixture(typeof(ChromeDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [Parallelizable]
    internal class DemoTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        [Test, Timeout(60000)]
        public void LoginTest()
        {
            var testUser1 = new PortalUser(CommonVar.TestRep1);
            var (Username, Driver, _) = DriverHelper.GetUsersDriver<TWebDriver>(testUser1.UserName);

            var commonStepsUser1 = new CommonSteps(Username, Driver);

            CommonSteps.Login(new List<CommonSteps>()
            {
                commonStepsUser1
            });
        }

        [Test, Timeout(60000)]
        [Retry(2)]
        [Category("GetHiredByAO")]
        public void InterviewTest()
        {
            var testUser1 = new PortalUser(CommonVar.Anon);
            var (Username, Driver, _) = DriverHelper.GetUsersDriver<TWebDriver>(testUser1.UserName);

            var homeStepsUser1 = new HomePageSteps(Username, Driver);

            homeStepsUser1.WhenIHaveClickedProductTypeCombo();
            homeStepsUser1.WhenIHaveSelectedProductType("Washing Machines");
            homeStepsUser1.WhenIHaveClickedSearchButton();

            var productsStepsUser1 = new ProductsPageSteps(Username, Driver);
            productsStepsUser1.WhenIHaveClickedFilterItem("Brand", "Hotpoint");
            productsStepsUser1.WhenIHaveClickedFilterItem("Wash Load", "9kg");
            productsStepsUser1.WhenIHaveClickedFilterItem("Energy Rating", "A+++");

            productsStepsUser1.ThenIHaveVerifedProductsFiltered("WashLoad", "7");
            productsStepsUser1.ThenIHaveVerifedProductsFiltered("Manufacturer", "Hotpoint");
            productsStepsUser1.ThenIHaveVerifedProductsFiltered("Energy Rating", "A+++");
        }

        [TearDown]
        public void AfterScenario()
        {
            BaseTest.AfterScenario();
        }

        [SetUp]
        public void BeforePortalScenario()
        {
            BaseTest.BeforePortalScenario<TWebDriver>();
        }
    }
}