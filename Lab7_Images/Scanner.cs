namespace TPL_Uebung;

/// <summary>
/// Die Scanner Klasse soll kontinuierlich einen Ordner überprüfen, ob neue Images zur Verarbeitung aufgetaucht sind.
/// Dafür soll diese Klasse intern einen Task besitzen, der diese Arbeit übernimmt.
/// Es soll auch sicher gestellt werden, das bereits gescannte/verarbeitete Images nicht doppelt gescannt/verarbeitet werden.
/// Der Benutzer soll die Möglichkeit haben, mehrere Scanner zu Erstellen und dadurch mehrere Ordner gleichzeitig zu Verarbeiten.
/// </summary>
public class Scanner : Runnable
{
	private string scanPath;

	public Scanner(string scanPath)
	{
		this.scanPath = scanPath;
        CurrentTask = new Task(Run);
	}

	protected override void Run()
	{
		while (Continue)
		{
			string[] imagePaths = Directory.GetFiles(scanPath);

			foreach (string p in imagePaths)
				if (!Program.ImagePaths.Contains(p))
					Program.ImagePaths.Enqueue(p);

			Thread.Sleep(1000);
		}
	}
}