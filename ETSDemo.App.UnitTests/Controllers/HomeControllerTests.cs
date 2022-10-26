using ETSDemo.App.Controllers;
using ETSDemo.App.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ETSDemo.App.UnitTests.Controllers
{
    [TestClass]
    [TestCategory("Controllers")]
    public class HomeControllerTests
    {
        private ILogger<HomeController> subLogger;
        private HttpContext context;
        private ICalculatorService calcSvc;
        private TelemetryClient telemetryClient;
        private IMemoryCache cache;
        private IConfiguration configuration;

        [TestInitialize]
        public void TestInitialize()
        {
            this.subLogger = Substitute.For<ILogger<HomeController>>();
            context = new DefaultHttpContext();
            context.TraceIdentifier = Guid.NewGuid().ToString();
            calcSvc = Substitute.For<ICalculatorService>();
            telemetryClient = new TelemetryClient(); // Substitute.For<TelemetryClient>();
            cache = Substitute.For<IMemoryCache>();
            configuration = Substitute.For<IConfiguration>();
        }

        private HomeController CreateHomeController()
        {
            var c = new HomeController(
                this.subLogger, this.calcSvc, telemetryClient, cache, configuration);
            c.ControllerContext.HttpContext = context;
            return c;
        }

        [TestMethod]
        public void Index_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = this.CreateHomeController();

            // Act
            var result = homeController.Index();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Privacy_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = this.CreateHomeController();

            // Act
            var result = homeController.Privacy();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Error_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = this.CreateHomeController();

            // Act
            var result = homeController.Error();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
