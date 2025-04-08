public class AddidtionOperation : IOperation
{
    public string Name => "Addition";
    public string Symbol => "➕";
    public double Execute(double a, double b) => a + b;
}
