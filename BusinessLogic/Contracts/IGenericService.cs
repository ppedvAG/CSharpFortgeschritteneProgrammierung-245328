namespace BusinessLogic.Contracts
{
    public interface IGenericService<T>
    {
        void Add(T car);
        List<T> GetAll();
        void Remove(T car);
        void Update(int index, T car);
    }
}