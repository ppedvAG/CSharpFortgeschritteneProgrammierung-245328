



namespace HelloDelegates
{
    internal class Program
    {
        // Function Deklaration fuer einen Function Type den wir spaeter benutzen werden
        public delegate void Hello(string name);

        static void Main(string[] args)
        {
            HelloDelegates();

            Console.WriteLine("\n\nActions und Funcs");
            ActionSamples();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        #region Delegates
        private static void HelloDelegates()
        {
            var hello = new Hello(HalloDeutschland);

            hello("Max Mustermann"); // Ausfuehrung des Delegates
            Console.WriteLine();


            // Mit += koennen wir weitere function pointers hinzufuegen
            hello += HalloDeutschland;
            hello("Alex");
            Console.WriteLine();

            hello += HelloAmerica;
            hello += HelloAmerica;
            hello("Pascale");
            Console.WriteLine();

            hello -= HelloAmerica;
            hello -= HelloAmerica;
            hello!("Tim"); // Mit ! sagen wir dem Compiler, dass hello garantiert nicht null sein kann!
            Console.WriteLine();

            hello -= HalloDeutschland;
            hello -= HalloDeutschland;
            //hello("Tim"); // NullReferenceExeption weil hello null sein wird

            if (hello != null)
            {
                hello("mit null check");
            }
            hello?.Invoke("Alternative mit ?");
        }

        private static void HelloAmerica(string name)
        {
            Console.WriteLine($"Howdy, my name is {name}!");
        }

        private static void HalloDeutschland(string name)
        {
            Console.WriteLine($"Hallo, mein Name ist {name}!");
        }
        #endregion

        private static void ActionSamples()
        {
            var printNumber = new Action<int, int>(PrintRandomNumber);

            printNumber(10, 20);
            // Besser mit null check
            printNumber?.Invoke(10, 20);

            var addNumber = new Func<int, int, int>((x, y) => x + y);
            int result = addNumber(10, 20);
            Console.WriteLine($"10 + 20 = {result}");

            bool IsEven(int number) => number % 2 == 0;

            var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var firstEvenNumber = numbers.ToList().Find(IsEven);
            Console.WriteLine("First even number is " + firstEvenNumber);
        }

        private static void PrintRandomNumber(int max, int min)
        {
            Console.WriteLine($"Random Number: {new Random().Next(max, min)}");
        }
    }
}
