using System.Drawing;
using System.Runtime.Versioning;

namespace TPL_Uebung;

/// <summary>
/// Die Worker Klasse soll kontinuierlich eine Queue überprüfen, ob diese weitere Images zur Verarbeitung enthält.
/// Falls diese Queue nicht leer ist, soll der Worker seine Arbeit beginnen. Diese Arbeit soll in einem Task durchgeführt werden.
/// Die Arbeit ist hier die Verarbeitung der Images über die ProcessImage Methode.
/// </summary>
public class Worker : Runnable
{
    public string OutputPath { get; }

    public Worker(string outputPath)
	{
		CurrentTask = new Task(Run);
        OutputPath = outputPath;
    }

    protected override void Run()
	{
		while (Continue)
		{
			if (Program.ImagePaths.TryDequeue(out string path))
			{
				Console.WriteLine($"Processing image: {path}");
				ProcessImage(path, $"{OutputPath}\\{path.Replace("Images", "")}");
				File.Delete(path);
				Console.WriteLine($"Processed image: {path}");
			}
		}
	}

    /// <summary>
    /// Diese Methode simuliert eine länger andauernde Arbeit (hier Bildverarbeitung) die mit paralleler Programmierung durchgeführt werden soll.
    /// Diese Methode nimmt ein gegebenes Image des Parameters loadPath und liest es ein.
    /// Danach wird das Image in Graustufen neu erzeugt und im Ordner savePath gespeichert.
    /// </summary>
    [SupportedOSPlatform("windows")] //Warnings entfernen
    private void ProcessImage(string loadPath, string savePath)
	{
		using Bitmap img = new Bitmap(loadPath);
		using Bitmap output = new Bitmap(img.Width, img.Height);
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
		output.Save(savePath); //Dateiname nicht vergessen
    }
}