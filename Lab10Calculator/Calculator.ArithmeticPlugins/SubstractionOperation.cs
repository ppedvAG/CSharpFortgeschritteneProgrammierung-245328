public class SubstractionOperation : IOperation
{
    public string Name => "Substraction";
    public string Symbol => "➖";
    public double Execute(double a, double b) => a - b;
}
