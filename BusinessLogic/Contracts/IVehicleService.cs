using BusinessLogic.Services;
using Serialization.Data;

namespace BusinessLogic.Contracts
{
    public interface IVehicleService
    {
        event EventHandler<DataChangedEventArgs<Car>> DataChanged;

        List<Car> Data { get; }

        void ShowInfo();
    }
}