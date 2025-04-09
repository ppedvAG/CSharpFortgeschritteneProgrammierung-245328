using System.Globalization;

namespace TermCalculator.Refactored
{
    public class Term
    {
        public string Input { get; }
        public double Number1 { get; private set; }
        public double Number2 { get; private set; }
        public CalcOperation Operation { get; private set; }

        public Term(string? input)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(Input))
                throw new ArgumentException("Eingabe darf nicht leer sein.");
        }

        public Term Parse()
        {
            Operation = GetCalcOperation();
            var numbers = SplitTerm();
            Number1 = ParseNumber(numbers[0]);
            Number2 = ParseNumber(numbers[1]);
            return this;
        }

        public double Calculate()
        {
            return Operation switch
            {
                CalcOperation.Addition => Number1 + Number2,
                CalcOperation.Subtraction => Number1 - Number2,
                CalcOperation.Multiplication => Number1 * Number2,
                CalcOperation.Division => Number2 != 0 ? Number1 / Number2 : throw new DivideByZeroException("Division durch Null ist nicht erlaubt."),
                _ => throw new ArgumentException("Ungültige Operation.")
            };
        }

        private CalcOperation GetCalcOperation()
        {
            return Input switch
            {
                var s when s.Contains('+') => CalcOperation.Addition,
                var s when s.Contains('-') => CalcOperation.Subtraction,
                var s when s.Contains('*') => CalcOperation.Multiplication,
                var s when s.Contains('/') => CalcOperation.Division,
                _ => throw new ArgumentException("Ungültiger Operator.")
            };
        }

        private string[] SplitTerm()
        {
            var separator = Operation switch
            {
                CalcOperation.Addition => '+',
                CalcOperation.Subtraction => '-',
                CalcOperation.Multiplication => '*',
                CalcOperation.Division => '/',
                _ => throw new ArgumentException("Ungültige Operation.")
            };

            var result = Input.Split(separator);
            return result.Length == 2 ? result : throw new FormatException("Ungültiges Eingabeformat.");
        }

        private double ParseNumber(string number)
        {
            number = number.Trim().Replace(',', '.');
            return double.TryParse(number.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                ? result
                : throw new FormatException($"Ungültige Zahl: {number}");
        }
    }
}
