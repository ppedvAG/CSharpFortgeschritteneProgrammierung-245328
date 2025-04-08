namespace BusinessLogic.Tests.Tests
{
    [TestClass]
    public sealed class CalculatorTests
    {
        [TestMethod]
        public void CrossTotal_12345_Returns15()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.CrossTotal(12345);

            // Assert
            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void CrossTotal_Negative12345_Returns15()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.CrossTotal(-12345);

            // Assert
            Assert.AreEqual(15, result);
        }

        [TestMethod]
        [DataRow(25, 12, 13)]
        [DataRow(5, 2, 3)]
        [DataRow(1, -12, 13)]
        public void Add_ValidNumbers_ReturnsExpected(int expected, int a, int b)
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Add(a, b);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Add_NumbersTooLarge_ThrowsOverflowException()
        {
            // Arrange
            var calculator = new Calculator();

            // Act & Assert
            Assert.ThrowsException<OverflowException>(() => calculator.Add(int.MaxValue, 1));
        }
    }
}
