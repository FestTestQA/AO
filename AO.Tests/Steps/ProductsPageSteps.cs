using AO.AutomationFramework.Core.GUI.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AO.Tests.Steps
{
    internal class ProductsPageSteps : BaseSteps
    {
        private readonly ProductsListerPage productsPage;

        internal ProductsPageSteps(string username, IWebDriver driver) : base(username, driver)
        {
            productsPage = new ProductsListerPage(driver);
        }

        internal void WhenIHaveClickedFilterItem(string filterGroup, string filterCriteria)
        {
            productsPage.FilterItem(filterGroup, filterCriteria).Click();
        }

        internal void ThenIHaveVerifedProductsFiltered(string filter, string filterCriteria)
        {
            switch (filter)
            {
                case "Energy Rating":
                    Assert.IsTrue(productsPage.CardBlocks.TrueForAll(cb => cb.EnergyRating.Contains(filterCriteria)));
                    break;

                case "Manufacturer":
                    Assert.IsTrue(productsPage.CardBlocks.TrueForAll(cb => cb.Manufacturer.Contains(filterCriteria)));
                    break;

                case "WashLoad":
                    Assert.IsTrue(productsPage.CardBlocks.TrueForAll(cb => cb.WashLoad >= float.Parse(filterCriteria)));
                    break;

                default:
                    break;
            }
        }
    }
}