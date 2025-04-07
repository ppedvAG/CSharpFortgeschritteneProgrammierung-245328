namespace HelloGenerics.SampleISP
{
    public class Person : Creature, IChef, IPerson
    {
        public Person()
        {
            
        }

        public Person(string name) : base(name)
        {
            
        }

        public TResult CookFood<T, TResult>(T value)
        {
            var result = default(TResult);
            Console.WriteLine($"Ich bereite {value} vor. Ergebnis: {result}");
            return result;
        }
    }
}
