namespace TermCalculator.Refactored
{
    public enum CalcOperation { Addition = 1, Subtraction, Multiplication, Division }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var input = GetInput();
                var term = new Term(input);
                var result = term.Parse().Calculate();

                Console.WriteLine($"\t={result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        public static string GetInput()
        {
            Console.WriteLine("Bitte gib einen Term mit zwei Zahlen und einem Grundrechenoperator (+ - * /) ein (z.B.: 25+13):");
            return Console.ReadLine() ?? throw new ArgumentNullException("Eingabe darf nicht null sein.");
        }
    }
}
