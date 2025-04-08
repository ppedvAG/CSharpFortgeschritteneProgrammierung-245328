using Lab9Personen;
using System.Text;

namespace Lab9_Personen.Extensions;

public static class ExtensionMethods
{
	public static string PrintTop10<T>(this IEnumerable<T> list, Func<T, string> formatter)
        => list.Take(10).ToString(formatter);

	public static string ToString<T>(this IEnumerable<T> list, Func<T, string> formatter)
    {
        return list.Aggregate(new StringBuilder(), AppendLine).ToString();

        StringBuilder AppendLine(StringBuilder agg, T p)
        {
            return agg.AppendLine(formatter(p));
        }
    }

    public static void ToConsole<T>(this T value, string prefix = "") => Console.WriteLine(prefix + value);
}
