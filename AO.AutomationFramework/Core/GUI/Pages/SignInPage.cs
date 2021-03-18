using OpenQA.Selenium;

namespace AO.AutomationFramework.Core.GUI.Pages
{
    public class SignInPage : PageBase
    {
        public IWebElement UsernameField
        {
            get { return FindElementById("username"); }
        }

        public IWebElement PasswordField
        {
            get { return FindElementById("password"); }
        }

        public IWebElement LogInButton
        {
            get { return FindElementById("btn-login"); }
        }

        public IWebElement SignUpButton
        {
            get { return FindElementById("btn-signup"); }
        }

        public IWebElement RestPasswordLink
        {
            get { return FindElementById("nav-request-password-reset"); }
        }

        public SignInPage(IWebDriver driver) : base(driver)
        {
        }
    }
}