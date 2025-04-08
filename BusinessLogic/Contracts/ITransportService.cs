using Serialization.Data;

namespace BusinessLogic.Contracts
{
    public interface ITransportService
    {
        void Load(string brandName);
        List<Car>? Unload();
        void ShowInfo();
    }
}