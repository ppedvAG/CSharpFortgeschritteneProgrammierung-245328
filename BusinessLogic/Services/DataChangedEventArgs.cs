namespace BusinessLogic.Services
{
    public class DataChangedEventArgs<T> : EventArgs
    {
        public T OldValue{ get; init; }

        public T NewValue{ get; init; }
    }
}
