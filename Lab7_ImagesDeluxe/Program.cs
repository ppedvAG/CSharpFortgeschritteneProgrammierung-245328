using System.Collections.Concurrent;

namespace TPL_ImagesDeluxe;

public class Program
{
	public static ConcurrentQueue<string> ImagePathQueue = new();
	public static ConcurrentBag<string> ProcessedImages = new();

	public static List<Scanner> Scanners = new();
	public static List<Worker> Workers = new();

	private static bool ProcessRunning = false;

	private static string LastOutput = "Keine";

	public static string OutputPath = "Output";

	private static bool SuspendRefresh = false;

	private static readonly Dictionary<ConsoleKey, Action> Inputs = new()
	{
		{ ConsoleKey.D1, CreateScanner },
		{ ConsoleKey.D2, AdjustWorkerAmount },
		{ ConsoleKey.D3, AdjustOutputPath },
		{ ConsoleKey.D4, StartProcess },
		{ ConsoleKey.D5, PauseProcess }
	};

	static void Main(string[] args)
	{
		CreateOutputTask();
		while (true)
		{
			ConsoleKey inputKey = Console.ReadKey(true).Key;
			if (Inputs.TryGetValue(inputKey, out Action a))
				ProcessInput(a, inputKey != ConsoleKey.D4 && inputKey != ConsoleKey.D5);

			#region Alternative
			//if (inputKey == ConsoleKey.D4 || inputKey == ConsoleKey.D5)
			//{
			//	value.Invoke();
			//	Console.Clear();
			//	continue;
			//}

			//SuspendRefresh = true;
			//value.Invoke();
			//Console.Clear();
			//SuspendRefresh = false;
			#endregion
		}

		void ProcessInput(Action a, bool suspend = false)
		{
			SuspendRefresh = suspend;
			a.Invoke();
			Console.Clear();
			SuspendRefresh = false;
		}
	}

	#region Print Methoden
	private static void PrintUserInputs()
	{
		Console.WriteLine("Eingaben: ");
		Console.WriteLine("1: Neuen Scanner erstellen");
		Console.WriteLine("2: Anzahl Worker Tasks anpassen");
		Console.WriteLine("3: Speicherpfad anpassen");
		Console.WriteLine("4: Prozess starten/fortsetzen");
		Console.WriteLine("5: Prozess pausieren");
	}

	private static void PrintStatus()
	{
		if (Scanners.Count != 0)
		{
			Console.WriteLine("\nScanner Liste: ");
			for (int i = 0; i < Scanners.Count; i++)
				Console.WriteLine($"{i}: {Scanners[i].ScanPath}");
		}

		if (Workers.Count != 0)
		{
			Console.WriteLine("\nWorker Liste: ");
			for (int i = 0; i < Workers.Count; i++)
				Console.WriteLine($"{i}: {Workers[i].CurrentPath}");
		}

		Console.WriteLine($"\nSpeicherpfad: {Path.GetFullPath(OutputPath)}");
		Console.WriteLine($"Letzte Meldung: {LastOutput}");
	}

	private static void CreateOutputTask()
	{
		Task.Run(() =>
		{
			while (true)
			{
				if (SuspendRefresh)
					continue;

				Console.SetCursorPosition(0, 0);
				PrintUserInputs();
				PrintStatus();
				Thread.Sleep(1000);
			}
		});
	}
	#endregion

	#region Input Methoden
	private static void CreateScanner()
	{
		Console.Write("Gib einen Pfad zum Scannen ein: ");
		string scannerEingabe = Console.ReadLine();

		if (Path.Exists(scannerEingabe))
			Scanners.Add(new Scanner(scannerEingabe));
		else
			LastOutput = "Kein valider Pfad eingegeben";
	}

	private static void AdjustWorkerAmount()
	{
		Console.Write($"Gib eine neue Anzahl von Worker-Tasks ein (derzeit {Workers.Count}): ");
		string workerEingabe = Console.ReadLine();
		if (int.TryParse(workerEingabe, out int newWorkers))
		{
			if (newWorkers > 0 && newWorkers != Workers.Count)
			{
				PauseList(Workers);

				int newAmount = newWorkers - Workers.Count;
				for (int i = 0; i < newAmount; i++)
					if (Workers.Count < newWorkers)
						Workers.Add(new Worker(ProcessRunning));

				if (newAmount < 0)
					Workers.RemoveRange(0, Workers.Count - newWorkers);
			}
			else
				LastOutput = "Ungültige Eingabe";
		}
	}

	private static void AdjustOutputPath()
	{
		Console.Write("Gib einen neuen Speicherpfad ein: ");
		string userPath = Console.ReadLine();

		if (Path.Exists(userPath))
			OutputPath = userPath;
		else
			LastOutput = "Kein valider Pfad eingegeben";
	}

	private static void StartProcess()
	{
		if (ProcessRunning)
		{
			LastOutput = "Prozess läuft bereits";
			return;
		}

		if (Scanners.Count == 0)
		{
			LastOutput = "Keine Scanner erstellt";
			return;
		}

		Scanners.ForEach(e => e.Continue = true);

		if (Workers.Count == 0)
		{
			for (int i = 0; i < 4; i++)
				Workers.Add(new Worker(true));
			LastOutput = "Prozess gestartet mit 4 Worker-Tasks";
		}
		else
		{
			Workers.ForEach(e => e.Continue = true);
			LastOutput = "Prozess gestartet";
		}

		if (!Directory.Exists(OutputPath))
			Directory.CreateDirectory(OutputPath);

		ProcessRunning = true;
	}

	private static void PauseProcess()
	{
		if (!ProcessRunning)
		{
			LastOutput = "Prozess läuft nicht";
			return;
		}

		PauseList(Scanners);
		PauseList(Workers);
		ProcessRunning = false;
		LastOutput = "Prozess pausiert";
	}
	#endregion

	private static void PauseList<T>(List<T> runnables) where T : Runnable
	{
		Console.WriteLine($"Warte auf das Beenden aller {typeof(T).Name}");
		foreach (Runnable r in runnables)
			r.Continue = false;

		while (runnables.Any(e => e.CurrentTask.Status == TaskStatus.Running))
			continue;
	}
}