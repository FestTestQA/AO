using OpenQA.Selenium;
using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using AO.AutomationFramework.Core.GUI;
using AO.AutomationFramework.Core.BusinessLogic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AO.AutomationFramework.Core.GUI.ControlTypes;

namespace AO.AutomationFramework.Core.GUI.Pages
{
    public class ProductsListerPage : PageBase
    {
        public ProductsListerPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement SideBar
        {
            get { return Driver.FindElement(By.ClassName("sidebar")); }
        }

        private IWebElement GetSideBarFilterGroup(string filterGroup)
        {
            var el = SideBar.FindElementsByAttributeStartsWith("div", "data-facet-name", filterGroup).ElementAtOrDefault(0);
            el.ScrollToView();
            return el;
        }

        public IWebElement FilterItem(string filterGroup, string filterCriteria)
        {
            WaitHelper.WaitFor(2000);
            return GetSideBarFilterGroup(filterGroup).FindElement(By.PartialLinkText(filterCriteria));
        }

        public List<CardBlock> CardBlocks
        {
            get
            {
                List<CardBlock> cardBlocks = new List<CardBlock>();
                var cardsBase = Driver.FindElementsByAttributeStartsWith("li", "data-testid", "product-card--standard").ToList();
                foreach (var card in cardsBase)
                {
                    cardBlocks.Add(new CardBlock(Driver, card));
                }
                return cardBlocks;
            }
        }
    }
}