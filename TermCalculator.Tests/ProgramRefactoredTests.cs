using Moq;
using TermCalculator.Refactored;
using UnitConverter.UI;

namespace TermCalculator.Tests.Vorgabe
{
    [TestClass]
    public class ProgramRefactoredTests
    {
        [TestMethod]
        public void Run_ValidInputUsingMoq_ShouldCalculate()
        {
            // Arrange
            string expectedPrompt = "Bitte gib einen Term mit zwei Zahlen und einem Grundrechenoperator (+ - * /) ein (z.B.: 25+13):";
            string expectedResult = "25+13\t=38";
            var consoleMock = new Mock<IConsole>();
            consoleMock.Setup(c => c.ReadLine())
                .Returns("25+13");

            // Act
            Program.Run(consoleMock.Object);

            // Assert
            consoleMock.Verify(c => c.WriteLine(expectedPrompt), Times.Once);
            consoleMock.Verify(c => c.WriteLine(expectedResult), Times.Once);

        }
    }
}
