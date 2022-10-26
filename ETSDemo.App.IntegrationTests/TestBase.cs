using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETSDemo.App.IntegrationTests
{
    public class TestBase
    {
        protected RemoteWebDriver Driver { get; private set; }
        protected int ImplicitWaitSeconds { get; private set;}

        public void TestInitialize(TestContext testContext)
        {
            int implicitWaitSeconds;
            if (!int.TryParse(testContext.Properties["implicitWaitSeconds"]?.ToString(), out implicitWaitSeconds))
                implicitWaitSeconds = 30;
            this.ImplicitWaitSeconds = implicitWaitSeconds;
            bool useSeleniumGrid;
            if (!bool.TryParse(testContext.Properties["useSeleniumGrid"]?.ToString(), out useSeleniumGrid))
                useSeleniumGrid = false;
            if(useSeleniumGrid)
            {
                var seleniumGridUrl = testContext.Properties["seleniumGridUrl"].ToString();
                Driver = GetSeleniumGridDriver(seleniumGridUrl);
            }
            else
            {
                Driver = GetChromeDriver();
            }
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ImplicitWaitSeconds);
        }

        private RemoteWebDriver GetChromeDriver()
        {
            var path = Environment.GetEnvironmentVariable("ChromeWebDriver");
            var options = new ChromeOptions();
            options.AddArguments("--no-sandbox");

            if (!string.IsNullOrWhiteSpace(path))
            {
                return new ChromeDriver(path, options, TimeSpan.FromSeconds(300));
            }
            else
            {
                return new ChromeDriver(options);
            }
        }

        private RemoteWebDriver GetSeleniumGridDriver(string seleniumGridUrl)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--headless");
            //chromeOptions.AddArgument("--whitelisted-ips");
            //chromeOptions.AddArgument("--no-sandbox");
            //chromeOptions.AddArgument("--disable-extensions");
            // chromeOptions.AddArgument("--start-maximized");
            return new RemoteWebDriver(new Uri(seleniumGridUrl), chromeOptions.ToCapabilities());
        }
    }
}
