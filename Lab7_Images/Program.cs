using System.Collections.Concurrent;

namespace TPL_Uebung;

internal class Program
{
    private const int DefaultWorkerCount = 4;
    private const string InputPath = "Images";
    private static string OutputPath = "Output";

    public static ConcurrentQueue<string> ImagePaths = [];

	public static List<Scanner> Scanners = [];

	public static List<Worker> Workers = [];

	static void Main(string[] args)
	{
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Eingaben: ");
            Console.WriteLine("1: Neuen Scanner erstellen");
            Console.WriteLine("2: Anzahl Worker Tasks anpassen");
            Console.WriteLine("3: Speicherpfad anpassen");
            Console.WriteLine("4: Prozess starten/fortsetzen");
            Console.WriteLine("5: Prozess pausieren");

            ConsoleKey inputKey = Console.ReadKey(true).Key;

            switch (inputKey)
            {
                case ConsoleKey.D1:
                    CreateScanner();
                    break;

                case ConsoleKey.D2:
                    AdjustWorkerAmount();
                    break;

                case ConsoleKey.D3:
                    AdjustOutputPath();
                    break;

                case ConsoleKey.D4:
                    StartProcess();
                    break;

                case ConsoleKey.D5:
                    PauseProcess();
                    break;
            }
        }
	}

    #region Input Methoden
    private static void CreateScanner()
    {
        Scanners.Add(new Scanner(InputPath));
        Console.WriteLine("Scanner wurde erstellt");
    }

    private static void AdjustWorkerAmount()
    {
        if (int.TryParse(Console.ReadLine(), out int amount) && amount > -1)
        {
            SetWorkerCount(amount);
        }
    }

    private static void SetWorkerCount(int count)
    {
        int newAmount = count - Workers.Count;

        if (newAmount < 0)
        {
            Console.WriteLine($"Removing {newAmount} workers");
            for (int i = 0; i < -newAmount; i++)
            {
                Workers.RemoveAt(Workers.Count - 1);
            }
        }
        else
        {
            Console.WriteLine($"Creating {newAmount} workers");
            for (int i = 0; i < newAmount; i++)
            {
                Workers.Add(new Worker(OutputPath));
            }
        }
    }

    private static void AdjustOutputPath()
    {
        var path = Console.ReadLine();
        if (Directory.Exists(path))
        {
            OutputPath = path;
        }
    }

    private static void StartProcess()
    {
        if (!Directory.Exists(OutputPath))
        {
            Directory.CreateDirectory(OutputPath);
        }

        if (!Workers.Any())
        {
            SetWorkerCount(DefaultWorkerCount);
        }

        Scanners.ForEach(w => w.Start());
        Workers.ForEach(w => w.Start());
    }

    private static void PauseProcess()
    {
        PauseList(Scanners);
        PauseList(Workers);
    }

    private static void PauseList<T>(List<T> runnables) where T : Runnable
    {
        Console.WriteLine($"Warte auf das Beenden aller {typeof(T).Name}");
        runnables.ForEach(r => r.Continue = !r.Continue);

        while (runnables.Any(e => e.CurrentTask.Status == TaskStatus.Running))
            continue;
    }
    #endregion
}