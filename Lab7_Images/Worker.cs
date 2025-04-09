using System.Drawing;
using System.Runtime.Versioning;

namespace TPL_Uebung;

/// <summary>
/// Die Worker Klasse soll kontinuierlich eine Queue überprüfen, ob diese weitere Images zur Verarbeitung enthält.
/// Falls diese Queue nicht leer ist, soll der Worker seine Arbeit beginnen. Diese Arbeit soll in einem Task durchgeführt werden.
/// Die Arbeit ist hier die Verarbeitung der Images über die ProcessImage Methode.
/// </summary>
public class Worker
{
    public Worker()
    {

	}

    /// <summary>
    /// Diese Methode simuliert eine länger andauernde Arbeit (hier Bildverarbeitung) die mit paralleler Programmierung durchgeführt werden soll.
    /// Diese Methode nimmt ein gegebenes Image des Parameters loadPath und liest es ein.
    /// Danach wird das Image in Graustufen neu erzeugt und im Ordner savePath gespeichert.
    /// </summary>
    [SupportedOSPlatform("windows")] //Warnings entfernen
    private void ProcessImage(string loadPath, string savePath)
    {
        Bitmap img = new Bitmap(loadPath);
        Bitmap output = new Bitmap(img.Width, img.Height);
        for (int i = 0; i < img.Width; i++)
        {
            for (int j = 0; j < img.Height; j++)
            {
                Color currentPixel = img.GetPixel(i, j);
                int grayScale = (int)(currentPixel.R * 0.3 + currentPixel.G * 0.59 + currentPixel.B * 0.11);
                Color newColor = Color.FromArgb(currentPixel.A, grayScale, grayScale, grayScale);
                output.SetPixel(i, j, newColor);
            }
        }
        output.Save(savePath); //Dateiname nicht vergessen
    }
}