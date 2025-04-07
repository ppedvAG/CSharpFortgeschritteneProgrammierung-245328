namespace HelloGenerics.SampleISP
{
    public interface IChef
    {
        TResult CookFood<T, TResult>(T value);
    }
}