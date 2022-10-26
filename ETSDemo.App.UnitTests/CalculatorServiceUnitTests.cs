using ETSDemo.App.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ETSDemo.App.UnitTests
{
    [TestClass]
    [TestCategory("Calculator")]
    public class CalculatorServiceUnitTests
    {

        private ICalculatorService _calcSvc;

        [TestInitialize]
        public void Initialize()
        {
            _calcSvc = new CalculatorService(new Microsoft.ApplicationInsights.TelemetryClient());
        }

        [TestMethod]
        public async Task CalculateTestAdd()
        {
            // Arrange
            string expression = "2 + 2";

            // Act
            double result = await _calcSvc.Calculate(expression);

            // Assert
            double expected = 4;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task CalculateTestSubtract()
        {
            // Arrange
            string expression = "2 - 2";

            // Act
            double result = await _calcSvc.Calculate(expression);

            // Assert
            double expected = 0;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task CalculateTestMultiply()
        {
            // Arrange
            string expression = "2 * 2";

            // Act
            double result = await _calcSvc.Calculate(expression);

            // Assert
            double expected = 4;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task CalculateTestDivide()
        {
            // Arrange
            string expression = "2 / 2";

            // Act
            double result = await _calcSvc.Calculate(expression);

            // Assert
            double expected = 1;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task CalculateTestDivide2()
        {
            // Arrange
            string expression = "3 / 2";

            // Act
            double result = await _calcSvc.Calculate(expression);

            // Assert
            double expected = 1.5;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow("1 + 2", 3)]
        [DataRow("10 / 5", 2)]
        [DataRow("1 + (2 * (3 * 9 - 4) + (2 / 5))", 47.4)]
        public async Task CalculateTest(string expression, double expected)
        {

            // Act
            double result = await _calcSvc.Calculate(expression);

            // Assert
            Assert.AreEqual(expected, result);
        }

    }
}
