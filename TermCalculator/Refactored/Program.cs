using UnitConverter.UI;

namespace TermCalculator.Refactored
{
    public enum CalcOperation { Addition = 1, Subtraction, Multiplication, Division }

    public class Program
    {
        public static void Main(string[] args)
        {
            Run(new ConsoleWrapper());
        }

        public static void Run(IConsole console)
        {
            try
            {
                var input = GetInput(console);
                var term = new Term(input);
                var result = term.Parse().Calculate();

                console.WriteLine($"\t={result}");
            }
            catch (Exception ex)
            {
                console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        public static string GetInput(IConsole console)
        {
            console.WriteLine("Bitte gib einen Term mit zwei Zahlen und einem Grundrechenoperator (+ - * /) ein (z.B.: 25+13):");
            return console.ReadLine() ?? throw new ArgumentNullException("Eingabe darf nicht null sein.");
        }
    }


}
