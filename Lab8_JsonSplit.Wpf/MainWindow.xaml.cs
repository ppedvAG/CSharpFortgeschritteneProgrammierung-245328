using System.Windows;

namespace Lab8_JsonSplit.Wpf
{
    public partial class MainWindow : Window
	{
        private readonly JsonParserService _service;

		/// <summary>
		/// Datei herunterladen, an einen beliebigen Ort speichern, entpacken und danach mit SaveSplitJson einlesen und bearbeiten.
		/// http://bulk.openweathermap.org/sample/history.city.list.min.json.gz
		/// </summary>
		public MainWindow()
        {
            InitializeComponent();

            _service = new(WriteText, (max) => Progress.Maximum = max);
        }

        /// <summary>
        /// Verwende diese Methode, um einen Text in der TextBox anzuzeigen.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="progress"></param>
        private void WriteText(string text)
        {
            Dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrEmpty(text))
                {
                    Output.Text += text;
                    Output.Text += Environment.NewLine;
                    Scroll.ScrollToEnd();
                }
                Progress.Value++;
            });
        }

        private async void SaveSplitJson(object sender, EventArgs e)
		{
            ButtonGrid.IsEnabled = false;

            await _service.SaveSplitJson();

            ButtonGrid.IsEnabled = true;
            Progress.Value = 0;
        }

		private async void LoadSplitJsonFiles(object sender, RoutedEventArgs e)
        {
            ButtonGrid.IsEnabled = false;

            await _service.LoadSplitJsonFiles();

            ButtonGrid.IsEnabled = true;
            Progress.Value = 0;
        }
	}
}
