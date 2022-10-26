using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETSDemo.App.IntegrationTests.PageObjects
{
    class HomePageObject : PageObjectBase
    {

        public const string WelcomeSelector = "body > div > main > div > h1";
        public const string HomeMenuXPath = "/html/body/header/nav/div/div/ul/li[1]/a";
        public const string CalculatorMenuXPath = "/html/body/header/nav/div/div/ul/li[2]/a";
        public const string PrivacyMenuXPath = "/html/body/header/nav/div/div/ul/li[3]/a";

        public HomePageObject(RemoteWebDriver driver) : base(driver)
        {
            this.Welcome = Driver.FindElementByCssSelector(WelcomeSelector);
            this.HomeMenu = Driver.FindElementByXPath(HomeMenuXPath);
            this.CalculatorMenu = Driver.FindElementByXPath(CalculatorMenuXPath);
            this.PrivacyMenu = Driver.FindElementByXPath(PrivacyMenuXPath);
        }

        public IWebElement Welcome { get; private set; }
        public IWebElement HomeMenu { get; private set; }
        public IWebElement CalculatorMenu { get; private set; }
        public IWebElement PrivacyMenu { get; private set; }
    }
}
