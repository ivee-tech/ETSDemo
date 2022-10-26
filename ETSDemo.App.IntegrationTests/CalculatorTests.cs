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
    public class CalculatorTests : TestBase
    {

        private static TestContext testContext;
        private CalculatorPageObject calcPO;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            CalculatorTests.testContext = testContext;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            base.TestInitialize(testContext);
            var webAppUrl = testContext.Properties["webAppUrl"]?.ToString();
            var calculatorUrl = $"{webAppUrl}/Home/Calculator";
            var nav = Driver.Navigate();
            nav.GoToUrl(calculatorUrl);
            calcPO = new CalculatorPageObject(Driver);
        }

        [TestMethod]
        public void TestLanding()
        {
            try
            {
                Assert.IsNotNull(calcPO.Calculator);
                var expectedText = "Calculator";
                Assert.AreEqual(expectedText, calcPO.Calculator.Text);

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
        [DataRow("1,+,1", 2)]
        [DataRow("1,3,-,4", 9)]
        [DataRow("2,*,3", 6)]
        [DataRow("1,0,0,/,2,5", 4)]
        [DataRow("1,+,2,*,(,4,-,6,)", -3)]
        [DataRow("4,*,(,3,-,4,/,2,)", 4)]
        public void TestCalculate(string ops, double expected)
        {
            try
            {
                var opKeys = ops.Split(",");
                foreach(var opKey in opKeys)
                {
                    calcPO.Ops[opKey].Click();
                }
                calcPO.ResultBtn.Click();
                calcPO.WaitForReady(ImplicitWaitSeconds);
                var value = calcPO.Result.GetAttribute("value");
                Assert.AreEqual(expected.ToString(), value);

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
