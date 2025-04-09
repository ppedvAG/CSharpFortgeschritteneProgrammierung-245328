using TermCalculator.Vorgabe;

namespace TermCalculator.Tests.Vorgabe
{
    [TestClass, TestCategory("Vorgabe")]
    public class ProgramTests
    {
        [TestMethod]
        public void CalculateTerm_Addition_ShouldCalculateCorrectly()
        {
            var term = new Term("25+13");
            Assert.AreEqual(38, Program.CalculateTerm(term));
        }

        [TestMethod]
        public void CalculateTerm_Subtraction_ShouldCalculateCorrectly()
        {
            var term = new Term("25-13");
            Assert.AreEqual(12, Program.CalculateTerm(term));
        }

        [TestMethod]
        public void CalculateTerm_Multiplication_ShouldCalculateCorrectly()
        {
            var term = new Term("25*13");
            Assert.AreEqual(325, Program.CalculateTerm(term));
        }

        [TestMethod]
        public void CalculateTerm_Division_ShouldCalculateCorrectly()
        {
            var term = new Term("26/13");
            Assert.AreEqual(2, Program.CalculateTerm(term));
        }

        [TestMethod]
        public void Term_ValidInput_ShouldParseCorrectly()
        {
            var term = new Term("25+13");
            Assert.AreEqual(25, term.Number1);
            Assert.AreEqual(13, term.Number2);
            Assert.AreEqual(CalcOperation.Addition, term.Operation);
        }

        [TestMethod]
        public void Term_NullInput_ShouldThrowNullReferenceException()
        {
            Assert.ThrowsException<NullReferenceException>(() => new Term(null));
        }

        [TestMethod]
        public void Term_EmptyInput_ShouldThrowNullReferenceException()
        {
            Assert.ThrowsException<NullReferenceException>(() => new Term(""));
        }

        [TestMethod]
        public void Term_InvalidNumber_ShouldThrowFormatException()
        {
            Assert.ThrowsException<FormatException>(() => new Term("abc+13"));
        }

        [TestMethod]
        public void Term_InvalidOperator_ShouldThrowNullReferenceException()
        {
            Assert.ThrowsException<NullReferenceException>(() => new Term("25%13"));
        }
    }
}
