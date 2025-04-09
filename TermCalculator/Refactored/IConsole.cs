namespace UnitConverter.UI
{
    public interface IConsole
    {
        string? ReadLine();
        void Write(string message = "");
        void WriteLine(string message = "");
    }
}