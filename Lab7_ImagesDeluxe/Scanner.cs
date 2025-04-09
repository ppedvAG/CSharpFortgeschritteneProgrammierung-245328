namespace TPL_ImagesDeluxe;

public class Scanner : Runnable
{
	public string ScanPath { get; private set; }

    public Scanner(string path)
    {
        ScanPath = path;
		CurrentTask = new Task(Run);
    }

	protected private override void Run()
	{
		while (Continue)
		{
			string[] pfade = Directory.GetFiles(ScanPath);
			foreach (string s in pfade)
			{
				if (Program.ProcessedImages.Contains(s) || Program.ImagePathQueue.Contains(s))
					continue;
				Program.ImagePathQueue.Enqueue(s);
			}

			//Directory.GetFiles(ScanPath)
			//	.Where(e => !Program.ProcessedImages.Contains(e) && !Program.ImagePathQueue.Contains(e))
			//	.ToList()
			//	.ForEach(e => Program.ImagePathQueue.Enqueue(e));

			Thread.Sleep(1000);
		}
	}
}