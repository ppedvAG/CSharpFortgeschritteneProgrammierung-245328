namespace TermCalculator.Vorgabe
{
    public enum CalcOperation { Addition = 1, Subtraction, Multiplication, Division }

    public class Program
    {
        public static void Main(string[] args)
        {
            var input = GetInput();

            var term = new Term(input);

            var result = CalculateTerm(term);

            Console.WriteLine($"\t={result}");
        }

        public static string? GetInput()
        {
            Console.WriteLine("Bitte gib einen Term mit zwei Zahlen und einem Grundrechenoperator (+ - * /) ein (z.B.: 25+13):");
            return Console.ReadLine();
        }

        public static double CalculateTerm(Term term)
        {
            switch (term.Operation)
            {
                case CalcOperation.Addition:
                    return term.Number1 + term.Number2;
                case CalcOperation.Subtraction:
                    return term.Number1 - term.Number2;
                case CalcOperation.Multiplication:
                    return term.Number1 * term.Number2;
                default:
                    return term.Number1 / term.Number2;
            }
        }
    }

    public class Term
    {
        public string Input { get; set; }
        public double Number1 { get; set; }
        public double Number2 { get; set; }
        public CalcOperation Operation { get; set; }

        public Term(string term)
        {
            Input = term;
            Operation = GetCalcOperation();

            string[] numbers = SplitTerm();

            Number1 = double.Parse(numbers[0]);
            Number2 = double.Parse(numbers[1]);
        }

        private CalcOperation GetCalcOperation()
        {
            if (Input.Contains('+'))
                return CalcOperation.Addition;
            else if (Input.Contains('-'))
                return CalcOperation.Subtraction;
            else if (Input.Contains('*'))
                return CalcOperation.Multiplication;
            else if (Input.Contains('/'))
                return CalcOperation.Division;
            else
                return 0;
        }

        private string[] SplitTerm()
        {
            switch (Operation)
            {
                case CalcOperation.Addition:
                    return Input.Split('+');
                case CalcOperation.Subtraction:
                    return Input.Split('-');
                case CalcOperation.Multiplication:
                    return Input.Split('*');
                case CalcOperation.Division:
                    return Input.Split('/');
            }
            return null;
        }
    }
}
