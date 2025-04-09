namespace TPL_Uebung;

public class Program
{
	static void Main(string[] args)
	{
		while (true)
		{
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

	}

	private static void AdjustWorkerAmount()
	{

	}

	private static void AdjustOutputPath()
	{

	}

	private static void StartProcess()
	{

	}

	private static void PauseProcess()
	{

	}
	#endregion
}