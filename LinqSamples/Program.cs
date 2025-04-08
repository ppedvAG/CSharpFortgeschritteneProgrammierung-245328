using LinqSamples.Extensions;
using Serialization.Data;
using System.Text;

namespace LinqSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var number = 4711;
            var digitSum = MathExtensions.DigitSum(number);
            
            // als Extension-Methode
            digitSum = number.DigitSum();

            Console.WriteLine($"Quersumme von {number} ist {digitSum}.\n");

            LinqSamples(Car.Generate(100));

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void LinqSamples(IEnumerable<Car> vehicles)
        {
            Console.WriteLine("Top 10 Vehicles");
            vehicles.Take(10)
                .ToList()
                .ForEach(Console.WriteLine);

            var averageSpeed = vehicles.Take(10).Average(v => v.TopSpeed);
            var maxSpeed = vehicles.Take(10).Max(v => v.TopSpeed);
            var minSpeed = vehicles.Take(10).Min(v => v.TopSpeed);
            Console.WriteLine($"Average Speed: {averageSpeed}km/h, Max Speed: {maxSpeed}km/h, Min Speed: {minSpeed}km/h");

            // Exception wenn Liste leer ist
            Console.WriteLine($"First vehicle: " + vehicles.First());

            // Default-Wert fuer LastOrDefault bei leerer Liste ist null
            Console.WriteLine($"Letztes vehicle: " + vehicles.LastOrDefault());

            // Wenn bei Single mehr als ein Element vorkommt fliegt eine Exception
            Console.WriteLine($"Single vehicle: " + vehicles.Skip(10).Take(1).Single());

            Console.WriteLine("\n\nAlle Fahrzeuge mit einem rotem Farbton");
            vehicles.Where(v => v.Color.ToString().Contains("Red"))
                .ToList()
                .ForEach(Console.WriteLine);

            Console.WriteLine("\n\nAutos sortieren nach TopSpeed und Model.");
            vehicles
                .OrderByDescending(v => v.TopSpeed)
                .ThenBy(v => v.Model)
                .Take(10)
                .ToList()
                .ForEach(Console.WriteLine);

            Console.WriteLine("\n\nAutos nach FuelType gruppieren");
            IEnumerable<IGrouping<string, Car>> groups = vehicles.GroupBy(v => v.Fuel);
            groups.Select(g => new { Fuel = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToList()
                .ForEach(g =>  Console.WriteLine($"{g.Count} Autos mit {g.Fuel}"));

            // Lokale Funktion
            //Func<StringBuilder, Car, StringBuilder> AppendLine =
            //  (sb, c) => sb.AppendLine($"Der {c.Color} {c.Model} faehrt max. {c.TopSpeed} km/h.");
            // vereinfacht
            static StringBuilder AppendLine(StringBuilder sb, Car c) 
                => sb.AppendLine($"\tDer {c.Color} {c.Model} faehrt max. {c.TopSpeed} km/h.");

            var startValue = new StringBuilder();
            var sb = vehicles.Skip(10).Take(10).Aggregate(startValue, AppendLine);
            Console.WriteLine(sb.ToString());

            Console.WriteLine("\n\nAutos nach Hersteller gruppieren");
            IEnumerable<KeyValuePair<string, string>> dictionary = vehicles.Take(20)
                .Select(c => new { Brand = c.Manufacturer, Vehicle = c })
                .GroupBy(v => v.Brand)
                .ToDictionary(g => g.Key, g => g.Select(v => v.Vehicle).Aggregate(new StringBuilder(), AppendLine).ToString());

            // Select wird an dieser Stelle noch nicht evaluiert weil IEnumberables "lazy" sind.
            var output = dictionary.Select(p =>
            {
                Console.Write($"{p.Key}: {p.Value}");
                return p;
            });

            // D. h. Linq Expressions werden erst bei ToList(), ToArray() oder ToDictionary() evaluiert
            Console.WriteLine("\n\nEs wurde noch nichts in die Console geschrieben. Aber nach ToArray() wird die Func ausgefuehrt.");
            _ = output.ToArray();
        }
    }
}
