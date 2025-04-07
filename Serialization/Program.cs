using Serialization.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;
using LegacyJson = Newtonsoft.Json;

namespace Serialization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cars = Car.Generate(10);

            Console.WriteLine("Xml Serialization");
            XmlSerialization(cars);

            Console.WriteLine("\nJson Serialization");
            JsonSerialization(cars);

            Console.WriteLine("\nJson with Newtonsoft");
            NewtonsoftSerialization(cars);

            Console.WriteLine("\nJson manuell deserialisieren");
            ManuellDeserialisieren(cars, "cars.json");
        }

        private static void XmlSerialization<T>(List<T> cars)
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            using (var stream = new FileStream("cars.xml", FileMode.Create))
            {
                serializer.Serialize(stream, cars);
            }

            using (var stream = new FileStream("cars.xml", FileMode.Open))
            {
                var deserialized = serializer.Deserialize(stream) as List<T>;

                foreach (var car in deserialized)
                {
                    Console.WriteLine(car);
                }
            }
        }

        private static void JsonSerialization(List<Car> cars)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // pretty print
                ReferenceHandler = ReferenceHandler.IgnoreCycles // rekursionen verhindern
            };
            var json = JsonSerializer.Serialize(cars, options);
            File.WriteAllText("cars.json", json);

            var fileContent = File.ReadAllText("cars.json");
            var result = JsonSerializer.Deserialize<List<Car>>(fileContent, options);

            foreach (var car in result)
            {
                Console.WriteLine(car);
            }
        }

        private static void NewtonsoftSerialization(List<Car> cars)
        {
            var settings = new LegacyJson.JsonSerializerSettings
            {
                Formatting = LegacyJson.Formatting.Indented,
                TypeNameHandling = LegacyJson.TypeNameHandling.Objects, // Vererbung ermoeglichen
                ReferenceLoopHandling = LegacyJson.ReferenceLoopHandling.Ignore,
            };
            var json = LegacyJson.JsonConvert.SerializeObject(cars, settings);
            File.WriteAllText("cars-newton.json", json);

            var fileContent = File.ReadAllText("cars-newton.json");
            var result = LegacyJson.JsonConvert.DeserializeObject<List<Car>>(fileContent, settings);

            foreach (var car in result)
            {
                Console.WriteLine(car);
            }
        }
        
        private static void ManuellDeserialisieren(List<Car> cars, string filename)
        {
            var json = File.ReadAllText(filename);
            
            // JSON-Dokument laden
            using JsonDocument document = JsonDocument.Parse(json);

            var root = document.RootElement;

            // Daten auslesen
            foreach (var item in root.EnumerateArray())
            {
                string manufacturer = item.GetProperty(nameof(Car.Manufacturer)).GetString();
                string model = item.GetProperty(nameof(Car.Model)).GetString();
                string color = item.GetProperty(nameof(Car.Color)).GetString();

                Console.WriteLine($"Manufacturer: {manufacturer}, Model: {model}, Color: {color}");
            }

        }
    }
}
