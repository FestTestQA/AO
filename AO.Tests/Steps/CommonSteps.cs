using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using AO.AutomationFramework.Core.BusinessLogic.Helpers;
using AO.AutomationFramework.Core.BusinessLogic.Variables;
using AO.AutomationFramework.Core.GUI.Pages;
using AO.Tests.DataModel;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AO.Tests.Steps
{
    internal class CommonSteps : BaseSteps
    {
        internal CommonSteps(string username, IWebDriver driver) : base(username, driver)
        {
        }

        internal void WhenIHaveWaitedFor(int mSeconds)
        {
            WaitHelper.WaitFor(mSeconds);
        }

        internal static void Login(List<CommonSteps> steps)
        {
            foreach (var step in steps)
            {
                step.Login();
            }
        }

        private void Login()
        {
            LoginAs(new PortalUser(Username));
        }

        private void LoginAs(PortalUser user)
        {
            var signInPage = new SignInPage(Driver);
            int counter = 10;
            do
            {
                signInPage.UsernameField.SendKeysJS(user.UserName);
                signInPage.PasswordField.SendKeysJS(user.PassWord);
                signInPage.LogInButton.JSClick();
                WaitHelper.WaitFor(2500);
                counter--;
            } while (Driver.Url.Contains("login") && counter > 0);

            DataModel.TestData.TestDataStorage.Instance.PortalUsers.Add(user);
        }

        internal void WhenIHaveBrowsed(string url)
        {
            Driver.Url = url;
        }

        internal void WhenIHaveBrowsedInNewTab(string url)
        {
            Driver.Navigate().GoToUrl(url);

            IWebElement body = Driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Control + 't');
        }

        internal void WhenIHaveBrowsedHomePage()
        {
            WhenIHaveBrowsed(CommonVar.GetSiteURL());
        }

        internal void WhenIHaveScrolledToBottom()
        {
            new PageBase(Driver).ScrollDown();
        }
    }
}