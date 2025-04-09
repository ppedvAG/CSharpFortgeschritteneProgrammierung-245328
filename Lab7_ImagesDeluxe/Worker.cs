using System.Drawing;
using System.Runtime.Versioning;

namespace TPL_ImagesDeluxe;

public class Worker : Runnable
{
	public string CurrentPath { get; private set; } = $"Wartet {new string(' ', Console.WindowWidth - 11)}";

	public Worker(bool start = false)
	{
		CurrentTask = new Task(Run);
		Continue = start;
	}

	protected private override void Run()
	{
		while (Continue)
		{
			if (!Program.ImagePathQueue.TryDequeue(out string path))
				continue;
			Program.ProcessedImages.Add(path);

			CurrentPath = path;
			ProcessImage(path, Program.OutputPath);
			CurrentPath = $"Wartet {new string(' ', Console.WindowWidth - 11)}";
		}
	}

	[SupportedOSPlatform("windows")] //Warnings entfernen
	private void ProcessImage(string loadPath, string savePath)
	{
		using var img = new Bitmap(loadPath);
        using var output = new Bitmap(img.Width, img.Height);
		for (int i = 0; i < img.Width; i++)
		{
			for (int j = 0; j < img.Height; j++)
			{
				Color currentPixel = img.GetPixel(i, j);
				int grayScale = (int) (currentPixel.R * 0.3 + currentPixel.G * 0.59 + currentPixel.B * 0.11);
				Color newColor = Color.FromArgb(currentPixel.A, grayScale, grayScale, grayScale);
				output.SetPixel(i, j, newColor);
			}
		}
		output.Save(Path.Combine(savePath, Path.GetFileName(loadPath)));
	}
}