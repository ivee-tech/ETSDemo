using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETSDemo.App.IntegrationTests.PageObjects
{
    class PageObjectBase
    {
        public RemoteWebDriver Driver { get; private set; }

        public PageObjectBase(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }
    }
}
