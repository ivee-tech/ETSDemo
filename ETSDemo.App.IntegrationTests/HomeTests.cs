using ETSDemo.App.IntegrationTests.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ETSDemo.App.IntegrationTests
{
    [TestClass]
    [TestCategory("UITests")]
    public class HomeTests : TestBase
    {

        private static TestContext testContext;
        private HomePageObject homePO;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            HomeTests.testContext = testContext;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            base.TestInitialize(testContext);
        }

        [TestMethod]
        public void TestHomePage()
        {
            try
            {
                var webAppUrl = testContext.Properties["webAppUrl"]?.ToString();
                Assert.IsNotNull(webAppUrl);
                var nav = Driver.Navigate();
                nav.GoToUrl(webAppUrl);
                homePO = new HomePageObject(Driver);

                Assert.IsNotNull(homePO.Welcome);
                var expectedText = "Welcome";
                Assert.AreEqual(expectedText, homePO.Welcome.Text);

                var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()) + ".png";
                var screenshot = Driver.GetScreenshot();
                screenshot.SaveAsFile(filePath);
                testContext.AddResultFile(filePath);
            }
            catch(Exception ex)
            {
                testContext.WriteLine(ex.ToString());
                throw;
            }
        }

        [TestMethod]
        public void TestPrivacyPage()
        {
            try
            {
                var webAppUrl = testContext.Properties["webAppUrl"]?.ToString();
                Assert.IsNotNull(webAppUrl);
                var nav = Driver.Navigate();
                nav.GoToUrl(webAppUrl);
                homePO = new HomePageObject(Driver);

                Assert.IsNotNull(homePO.PrivacyMenu);
                var expectedText = "Privacy";
                Assert.AreEqual(expectedText, homePO.PrivacyMenu.Text);

                homePO.PrivacyMenu.Click();
                var privacyPO = new PrivacyPageObject(Driver);
                expectedText = "Privacy Policy";
                Assert.AreEqual(expectedText, privacyPO.Privacy.Text);

                var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()) + ".png";
                var screenshot = Driver.GetScreenshot();
                screenshot.SaveAsFile(filePath);
                testContext.AddResultFile(filePath);
            }
            catch (Exception ex)
            {
                testContext.WriteLine(ex.ToString());
                throw;
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Driver != null)
            {
                Driver.Quit();
            }
        }
    }
}
