namespace LinqSamples.Extensions
{
    // 1. Klasse static machen
    internal static class MathExtensions
    {
        // Quersumme berechnen
        // 2. Fuer ersten Parameter das this Keyword verwenden
        public static int DigitSum(this int number)
        {
            // Ein string ist ein character array weswegen wir hier Sum von Linq verwenden koennen
            return number.ToString()
                .Sum(c => (int)char.GetNumericValue(c));
        }
    }
}
