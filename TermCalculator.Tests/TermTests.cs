using TermCalculator.Refactored;

namespace TermCalculator.Tests.Refactored
{
    [TestClass, TestCategory("Refactored")]
    public class TermTests
    {
        private const double DeltaAccuracy = 0.001;

        [TestMethod]
        [DataRow("25+13", 38)]
        [DataRow("25-13", 12)]
        [DataRow("25*13", 325)]
        [DataRow("26/13", 2)]
        [DataRow("25,5+13,3", 38.8)]
        [DataRow("25.5-13.3", 12.2)]
        [DataRow("25,5*13.3", 339.15)]
        [DataRow("26,6/13,3", 2)]
        public void Calculate_ValidInput_ShouldCalculateCorrectly(string input, double expected)
        {
            var term = new Term(input).Parse();
            Assert.AreEqual(expected, term.Calculate(), DeltaAccuracy);
        }

        [TestMethod]
        public void Term_InvalidInput_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Term(null));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("25%13")]
        public void Term_InvalidInput_ShouldThrowArgumentException(string input)
        {
            Assert.ThrowsException<ArgumentException>(() => new Term(input).Parse());
        }

        [TestMethod]
        [DataRow("abc+13")]
        public void Parse_InvalidInput_ShouldThrowFormatException(string input)
        {
            var term = new Term(input);
            Assert.ThrowsException<FormatException>(term.Parse);
        }

        [TestMethod]
        [DataRow("26/0")]
        public void Calculate_DivisionByZero_ShouldThrowDivideByZeroException(string input)
        {
            var term = new Term(input).Parse();
            Assert.ThrowsException<DivideByZeroException>(() => term.Calculate());
        }
    }

}
