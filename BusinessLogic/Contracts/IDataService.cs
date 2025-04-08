using BusinessLogic.Services;

namespace BusinessLogic.Contracts
{
    public interface IDataService<T>
    {
        event EventHandler<DataChangedEventArgs<T>> DataChanged;

        List<T> Data { get; }
    }
}