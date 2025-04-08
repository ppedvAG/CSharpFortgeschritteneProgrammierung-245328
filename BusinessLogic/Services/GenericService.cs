namespace BusinessLogic.Services
{
    public class GenericService<T>
    {
        public List<T> Data { get; }


        public event EventHandler<DataChangedEventArgs<T>> DataChanged;

        public GenericService(List<T> initialData)
        {
            Data = initialData;
        }

        public List<T> GetAll()
        {
            return Data;
        }

        public void Add(T car)
        {
            Data.Add(car);
        }

        public void Update(int index, T car)
        {
            var old = Data[index];
            Data[index] = car;

            DataChanged?.Invoke(this, new DataChangedEventArgs<T> { OldValue = old, NewValue = car });
        }

        public void Remove(T car)
        {
            Data.Remove(car);
        }
    }
}
