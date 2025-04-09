using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TPL_Multitasking.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(50);
                progressBar.Value = i;
                Output.Text += i + Environment.NewLine;
            }
        }

        private void StartTaskRun(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(50);

                    // UI Updates duerfen nicht von Side Thread/Tasks ausgefuehrt werden
                    // Dispatcher ist ein Property auf jedem WPF Element und ermoeglicht
                    // uns auf dem Thread der Komponente beliebigen Code auszufuehren
                    Dispatcher.Invoke(() =>
                    {
                        progressBar.Value = i;
                        Output.Text += i + Environment.NewLine;
                    });
                }
            });
        }

        private readonly CancellationTokenSource _cts = new();

        private async void StartAsyncAwait(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.IsEnabled = false;

                // Mit async/await kann der Code vereinfacht werden
                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        // Wir uebergeben den CancellationToken, damit der Task abgebrochen werden kann
                        await Task.Delay(50, _cts.Token);
                        progressBar.Value = i;
                        Output.Text += i + Environment.NewLine;
                    }
                }
                catch(TaskCanceledException)
                {
                    Output.Text += "Task wurde vom Benutzer abgebrochen" + Environment.NewLine;
                }
                finally
                {
                    btn.IsEnabled = true;
                }
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
        }

        // Zertifikat abgelaufen
        //const string RequestUrl = "http://www.gutenberg.org/files/54700/54700-0.txt";
        const string RequestUrl = "https://dummyjson.com/products?limit=5000";

        private async void SendRequest(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {

                btn.IsEnabled = false;

                try
                {
                    using (HttpClient client = new())
                    {
                        Output.Text = "Request gestartet\n";
                        var response = await client.GetAsync(RequestUrl, _cts.Token);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            Output.Text += "Response auslesen\n";
                            Output.Text += await response.Content.ReadAsStringAsync(_cts.Token);
                        }
                        else
                        {
                            Output.Text += "Fehler: " + response.ReasonPhrase;
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    Output.Text += "Task wurde vom Benutzer abgebrochen" + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    Output.Text += "Fehler: " + ex.Message;
                }
                finally
                {
                    // Wird automatisch dank dem using aufgerufen und muessen es nicht manuell aufrufen
                    //client.Dispose();
                }
                
                btn.IsEnabled = true;
            }
        }

        #region Mit Legacy Code umgehen

        /// <summary>
        /// Simuliert Legacy Methode welche sehr langsam ist und wir nicht anfassen duerfen
        /// weil keine Tests existieren und beim Refactoring kaputt gehen kann.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double CalcValuesVerySlowly(double input)
        {
            Thread.Sleep(5000);

            return 3.14159 * input;
        }

        // Schritt 1: Async Variante machen
        public Task<double> CalcValuesVerySlowlyAsync(double input)
            => Task.Factory.StartNew(o => CalcValuesVerySlowly((double)o), input);

        private async void StartLegacyCode(object sender, RoutedEventArgs e)
        {
            try
            {
                Output.Text = "Anfrage gestartet fuer 42\n";

                // Schritt 2: Warten bis der Task fertig ist
                // Hinweis zu await: TPL erstellt NICHT zwangslaeufig jedes mal einen Thread
                // sondern die Methode jedes await "erzeugt" eine sog. Co-Routine.
                // Die TPL verwaltet alle Co-Routines in einer Statemachine. (siehe https://de.wikipedia.org/wiki/Endlicher_Automat)
                double result = await CalcValuesVerySlowlyAsync(42);
                
                Output.Text += "Ergebnis: " + result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Da ist was schief gelaufen! \n{ex.Message}");
            }
        }

        #endregion
    }
}