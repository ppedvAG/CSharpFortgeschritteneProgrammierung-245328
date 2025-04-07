namespace Labs
{
    public class PrimeMain
    {
        public static void Run(string[] args)
        {
            PrimeComponent pc = new PrimeComponent();
            pc.Prime += (i) => Console.WriteLine($"Primzahl: {i}");
            pc.Prime100 += (i) => Console.WriteLine($"Hundertste Primzahl: {i}");
            pc.NotPrime += (i, div) => Console.WriteLine($"Keine Primzahl: {i}, teilbar durch {div}");
            pc.StartProcess();
        }
    }
}