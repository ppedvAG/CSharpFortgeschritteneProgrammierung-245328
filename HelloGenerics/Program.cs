
using HelloGenerics.Sample1;
using HelloGenerics.SampleISP;

namespace HelloGenerics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            StackSample();

            Console.WriteLine("\n\nCreature Sample\n");
            CreatureSample();

            Console.ReadKey();
        }

        private static void StackSample()
        {
            var simpleStack = new Stack();

            simpleStack.Push(42);
            simpleStack.Push(-1);
            simpleStack.Push(3.14);

            object item = simpleStack.Pop();
            Console.WriteLine("Item from stack " + item);

            // Berechne was mit dem Inhalt vom Stack
            var result = 2 * (double)item;

            // Problem: Zur Laufzeit kann eine InvalidCastException auftreten
            // Fail Fast Prinzip: Fehler sollten so frueh wie moeglich auftreten, d. h. am besten bereits zur Compile-Zeit.
            // Mit Generics kann keine InvalidCastException mehr auftreten
            var dateStack = new GenericStack<DateTime>();
            dateStack.Push(new DateTime(2022, 1, 1));
            dateStack.Push(new DateTime(2022, 1, 2));
            dateStack.Push(DateTime.Now);

            var numberStack = new GenericStack<double>();
            numberStack.Push(1.0);
            numberStack.Push(2.0);
            numberStack.Push(3.14159);

            var number = numberStack.Pop();
            var date = dateStack.Pop();
            Console.WriteLine("Item from generic stack + 2: " + (number + 2.0));

        }

        private static void CreatureSample()
        {
            var bunny = new Creature
            {
                Name = "Bunny 🐰",
                FavoriteFood = "Carrot 🥕🥕"
            };

            // Wir wollen mit Abstraktionen (Interfaces bzw. sog. Contracts) arbeiten weil wir
            // nicht zwischen Creature oder Person unterscheiden wollen
            EatSomething(bunny);

            var person = new Person
            {
                Name = "John Doe",
                FavoriteFood = "Pizza 🍕🍕"
            };
            DoWork(person);

            // Bessere Alternative: Wir verwenden sog. Constraints
            // Um mehrere Interfaces ohne ein "super-Interface" definieren zu muessen
            // koennen wir Constraints verwenden
            DoDailyStuff(bunny);

            Console.WriteLine("\nMit constraints Klassen dynamisch erzeugen");
            var duffyDuck = CreateCreature<Creature>("Duffy 🦆", "Wuermer 🪱");
            EatSomething(duffyDuck);


            var oldStuff = CreateCreatureOld<Creature>("Old creature", "🥪🥪");
            EatSomething(oldStuff);
        }

        private static void EatSomething(IEat eater)
        {
            eater.Eat();
        }

        private static void DoWork(IPerson person)
        {
            person.CookFood<string, object>("🍕🍕");
            person.Eat();
            person.Swim();
        }

        private static void DoDailyStuff<T>(T creature) 
            where T : IEat, ISwim
        {
            creature.Eat();
            creature.Swim();
        }

        private static T CreateCreature<T>(string name, string food) 
            where T : class, IEat, new()
        {
            var creature = new T();
            creature.FavoriteFood = food;
            return creature;
        }

        // Als es noch das new()-Constraint nicht gab...
        private static T CreateCreatureOld<T>(string name, string food)
        {
            // Reflection
            Type creatureType = typeof(T);

            // Vorteil von dieser Methode: Wir koennen Parameter uebergeben
            object obj = Activator.CreateInstance(creatureType, name);
            var creature = (T)obj;
            return creature;
        }
    }
}
