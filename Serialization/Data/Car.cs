using Bogus;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Serialization.Data
{
    public class Car
    {
        [XmlIgnore]
        [JsonIgnore]
        public long Id { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        [XmlAttribute("VehicleType")]
        [JsonPropertyName("VehicleType")]
        public string Type { get; set; }

        public string Fuel { get; set; }

        public int TopSpeed { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public KnownColor Color { get; set; }

        public override string ToString()
        {
            return $"{Manufacturer,-20} {Model,-20} {Type,-20} {Fuel,-20} {Color,-20} {TopSpeed} km/h max";
        }

        public static List<Car> Generate(int count = 20)
        {
            KnownColor[] exclude = Enumerable.Range(1, 27)
                .Select(i => (KnownColor)i).ToArray();

            return new Faker<Car>()
                .UseSeed(42)
                .RuleFor(c => c.Id, f => f.Random.Long())
                .RuleFor(c => c.Manufacturer, f => f.Vehicle.Manufacturer())
                .RuleFor(c => c.Model, f => f.Vehicle.Model())
                .RuleFor(c => c.Type, f => f.Vehicle.Type())
                .RuleFor(c => c.Fuel, f => f.Vehicle.Fuel())
                .RuleFor(c => c.TopSpeed, f => f.Random.Number(10, 25) * 10)
                .RuleFor(c => c.Color, f => f.Random.Enum(exclude))
                .Generate(count);
        }
    }
}
