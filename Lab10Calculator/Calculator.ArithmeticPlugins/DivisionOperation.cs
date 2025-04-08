public class DivisionOperation : IOperation
{
    public string Name => "Division";
    public string Symbol => "➗";
    public double Execute(double a, double b) => b != 0 ? a / b : double.NaN;
}
