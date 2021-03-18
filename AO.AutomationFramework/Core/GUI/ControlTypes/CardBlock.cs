using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace AO.AutomationFramework.Core.GUI.ControlTypes
{
    public class CardBlock : ControlBase
    {
        private string manufacturer;

        private string energyRating;

        private float washLoad;

        public CardBlock(IWebDriver webDriver, IWebElement root) : base(webDriver, root)
        {
            SetCardBlock();
        }

        public string Manufacturer { get => manufacturer; set => manufacturer = value; }

        public string EnergyRating { get => energyRating; set => energyRating = value; }

        public float WashLoad { get => washLoad; set => washLoad = value; }

        private void SetCardBlock()
        {
            energyRating = WebElement.FindElementsByAttributeStartsWith("strong", "data-testid", "energy-rating-link-updated-component").ElementAtOrDefault(0).Text;
            manufacturer = WebElement.FindElement(By.ClassName("product-card__brand-logo")).GetAttribute("alt");
            var bullet = BulletItems.FirstOrDefault(b => b.Text.Contains("drum"));
            washLoad = float.Parse(bullet.Text.Substring(0, bullet.Text.IndexOf("kg")));
        }

        private List<IWebElement> BulletItems
        {
            get
            {
                var productBulletList = WebElement.FindElement(By.ClassName("product-card__bullet-list--rebrand"));
                return productBulletList.FindElements(By.ClassName("ml-2")).ToList();
            }
        }
    }
}