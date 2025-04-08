using BusinessLogic.Attributes;
using BusinessLogic.Contracts;
using Serialization.Data;

namespace BusinessLogic.Services
{
    [Service("Fahrzeugverwaltung", 1)]
    public class VehicleService : GenericService<Car>, IVehicleService
    {
        private readonly string? _state;

        public VehicleService() : base(Car.Generate())
        {
            _state = "Top Secret";
        }

        public Car CreateCar(string modelName, string brandName)
        {
            var car = new Car
            {
                Model = modelName,
                Manufacturer = brandName
            };

            Add(car);
            return car;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Anzahl Elemente: {Data.Count}\tStatus: {_state}");
        }
    }
}
