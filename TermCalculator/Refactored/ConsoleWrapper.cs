namespace UnitConverter.UI
{
    public class ConsoleWrapper : IConsole
    {
        public void Write(string message) => Console.Write(message);

        public void WriteLine(string message) => Console.WriteLine(message);

        public string? ReadLine() => Console.ReadLine();
    }
}
