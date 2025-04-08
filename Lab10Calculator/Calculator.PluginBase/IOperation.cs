

public interface IOperation
{
    string Name { get; }
    string Symbol { get; }
    double Execute (double a, double b);    
}
