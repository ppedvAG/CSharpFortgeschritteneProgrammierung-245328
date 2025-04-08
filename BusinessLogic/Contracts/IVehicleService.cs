using Serialization.Data;

namespace BusinessLogic.Contracts
{
    public interface IVehicleService : IGenericService<Car>
    {
        Car CreateCar(string modelName, string brandName);

        void ShowInfo();
    }
}