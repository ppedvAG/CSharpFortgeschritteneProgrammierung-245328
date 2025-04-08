using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogic
{
    public class Calculator
    {
        public int Add(int a, int b)
        {
            // Fehler beheben und OverflowException zulassen mit checked
            checked
            {
                return a + b;
            }
        }

        public int CrossTotal(int number)
        {
            // Fehler behoben: Negative Zahlen werden wie Positive behandelt
            int n = number < 0 ? number * -1 : number;

            // Ein string ist ein character array weswegen wir hier Sum von Linq verwenden koennen
            return n.ToString()
                .Sum(c => (int)char.GetNumericValue(c));
        }
    }
}
