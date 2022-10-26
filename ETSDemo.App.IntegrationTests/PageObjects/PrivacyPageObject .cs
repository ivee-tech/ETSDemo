using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETSDemo.App.IntegrationTests.PageObjects
{
    class PrivacyPageObject : PageObjectBase
    {

        public const string PrivacySelector = "body > div > main > h1";

        public PrivacyPageObject(RemoteWebDriver driver) : base(driver)
        {
            this.Privacy = Driver.FindElementByCssSelector(PrivacySelector);
        }

        public IWebElement Privacy { get; private set; }
    }
}
