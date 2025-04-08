public class MultiplicationOperation : IOperation
{
    public string Name => "Multiplication";
    public string Symbol => "✖️";
    public double Execute(double a, double b) => a * b;
}