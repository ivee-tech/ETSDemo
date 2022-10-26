using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETSDemo.App.IntegrationTests.PageObjects
{
    class CalculatorPageObject : PageObjectBase
    {

        public const string CalculatorSelector = "body > div > main > div > h1";
        public const string ResultId = "txtResult";
        public const string AddId = "add";
        public const string SubId = "sub";
        public const string MulId = "mul";
        public const string DivId = "div";
        public const string ObId = "ob"; // open bracket button
        public const string CbId = "cb"; // closed bracket button
        public const string ResultBtnId = "result";
        
        public CalculatorPageObject(RemoteWebDriver driver) : base(driver)
        {
            this.Calculator = Driver.FindElementByCssSelector(CalculatorSelector);
            this.Result = Driver.FindElementById(ResultId);
            this.Buttons = new List<IWebElement>();
            for(var i = 0; i < 10; i++)
            {
                Buttons.Add(Driver.FindElementById(i.ToString()));
            }
            Add = Driver.FindElementById(AddId);
            Sub = Driver.FindElementById(SubId);
            Mul = Driver.FindElementById(MulId);
            Div = Driver.FindElementById(DivId);
            Ob = Driver.FindElementById(ObId);
            Cb = Driver.FindElementById(CbId);
            Ops = new Dictionary<string, IWebElement>(){ { "+", Add }, { "-", Sub }, { "*", Mul }, { "/", Div }, { "(", Ob }, { ")", Cb } };
            for(var i = 0; i < 10; i++)
            {
                Ops.Add(i.ToString(), Buttons[i]);
            }
            this.ResultBtn = Driver.FindElementById(ResultBtnId);
        }

        public IWebElement Calculator { get; private set; }
        public IWebElement Result { get; private set; }
        public List<IWebElement> Buttons { get; private set; }
        public IWebElement Add { get; private set; }
        public IWebElement Sub { get; private set; }
        public IWebElement Mul { get; private set; }
        public IWebElement Div { get; private set; }
        public IWebElement Ob { get; private set; }
        public IWebElement Cb { get; private set; }
        public IWebElement ResultBtn { get; private set; }

        public IDictionary<string, IWebElement> Ops { get; private set; } = new Dictionary<string, IWebElement>();

        public void WaitForReady(int waitSeconds)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds));
            wait.Until(driver =>
            {
                bool isSpinnerHidden = (bool)((IJavaScriptExecutor)driver).
                    ExecuteScript("return isSpinnerHidden()");
                return isSpinnerHidden;
            });
        }

    }
}
