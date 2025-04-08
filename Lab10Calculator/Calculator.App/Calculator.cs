namespace Calculator.App
{
    public class Calculator
    {
        public static void Run(IOperation[] operations)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("═════════ Taschenrechner mit Plugins ═════════");
                Console.ResetColor();

                // Menüanzeige
                Console.WriteLine("\nVerfügbare Operationen:");
                for (int i = 0; i < operations.Length; i++)
                {
                    Console.WriteLine($"  [{i + 1}] {operations[i].Name,-16} ({operations[i].Symbol})");
                }
                Console.WriteLine($"  [0] Beenden\n");

                // Operation auswählen
                var choice = ReadIntInput("Ihre Wahl: ", operations.Length);

                if (choice == 0) break;

                // Zahlen eingeben
                double n1 = ReadDoubleInput("Erste Zahl:  ");
                double n2 = ReadDoubleInput("Zweite Zahl: ");

                // Berechnung durchführen
                var operation = operations[choice - 1];
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{n1} {operation.Symbol} {n2} = {operation.Execute(n1, n2)}\n");
                Console.ResetColor();

                Console.WriteLine("Beliebige Taste für nächste Berechnung...");
                Console.ReadKey();
            }
        }

        static double ReadDoubleInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out var result))
                    return result;

                ShowError("Ungültige Zahl!");
            }
        }

        static int ReadIntInput(string prompt, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out var result) && result <= max)
                    return result;

                ShowError($"Bitte Zahl zwischen 0 und {max} eingeben!");
            }
        }

        static void ShowError(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Fehler: {message}");
            Console.ForegroundColor = originalColor;
        }
    }
}
