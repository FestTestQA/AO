using AO.AutomationFramework.Core.BusinessLogic.Extensions;
using AO.AutomationFramework.Core.BusinessLogic.Helpers;
using AO.AutomationFramework.Core.GUI.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AO.Tests.Steps
{
    internal abstract class BaseSteps
    {
        private readonly Dictionary<Type, PageBase> PageInstances = new Dictionary<Type, PageBase>();

        protected IWebDriver Driver;

        protected HomePage HomePage;

        protected string Username;

        internal BaseSteps(string username, IWebDriver driver)
        {
            Username = username;
            Driver = driver;
            HomePage = GetPage<HomePage>();
        }

        internal void WhenIClickBrowserBackButton()
        {
            WaitHelper.WaitFor(3000);
            Driver.Navigate().Back();
        }

        protected void ThenIHaveVerifedElementIsInViewPort(IWebElement webElement)
        {
            Assert.IsTrue(webElement.IsElementInViewPort());
        }

        internal IWebElement TryFindPageElement(Func<IWebElement> elementGetter, int timeout)
        {
            var task = Task.Factory.StartNew(() =>
            {
                IWebElement element = null;
                do
                {
                    try
                    {
                        element = elementGetter();
                    }
                    catch
                    { element = null; }
                } while (element == null);
                return element;
            }, TaskCreationOptions.LongRunning);
            return task.Wait(TimeSpan.FromMilliseconds(timeout)) ? task.Result : null;
        }

        internal List<IWebElement> TryFindPageElements(Func<List<IWebElement>> elementsGetter, int timeout)
        {
            var task = Task.Factory.StartNew(() =>
            {
                List<IWebElement> elements = null;
                do
                {
                    try
                    {
                        elements = elementsGetter();
                    }
                    catch
                    { elements = null; }
                } while (elements == null);
                return elements;
            }, TaskCreationOptions.LongRunning);
            return task.Wait(TimeSpan.FromMilliseconds(timeout)) ? task.Result : null;
        }

        internal T GetPage<T>() where T : PageBase
        {
            try
            {
                return (T)PageInstances[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                PageInstances.Add(typeof(T), (T)Activator.CreateInstance(typeof(T), Driver));
                return GetPage<T>();
            }
        }
    }
}