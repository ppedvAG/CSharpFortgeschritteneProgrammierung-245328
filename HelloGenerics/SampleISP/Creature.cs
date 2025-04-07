namespace HelloGenerics.SampleISP
{
    /// <summary>
    /// ISP: Interface Segregation Principle besagt: 
    /// "Clients shouldn't be forced to depend on interfaces they don't use."
    /// D. h. nur die Interfaces implementieren, die wirklich gebraucht werden.
    /// </summary>
    public class Creature : IEat, ISwim
    {
        public string Name { get; set; }

        public string FavoriteFood { get; set; }

        // Default-Konstruktor notwendig um das new()-Constraint verwenden zu koennen
        public Creature()
        {            
        }

        public Creature(string name)
        {
            Name = name;
        }

        public void Eat()
        {
            Console.WriteLine($"{Name} is eating {FavoriteFood}");
        }

        public void Swim()
        {
            Console.WriteLine($"{Name} is swimming");
        }
    }
}
