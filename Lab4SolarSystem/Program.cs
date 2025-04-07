using Lab_SolarSystem.Data;

namespace Lab_SolarSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Node<SolarItem> sunNode = new Node<SolarItem>(new SolarItem("Sonne", SolarItemType.Star));

            Node<SolarItem> merkurNode = new Node<SolarItem>(new SolarItem("Merkur", SolarItemType.Planet), sunNode);

            Node<SolarItem> venusNode = new Node<SolarItem>(new SolarItem("Venus", SolarItemType.Planet), sunNode);

            Node<SolarItem> earthNode = new Node<SolarItem>(new SolarItem("Erde", SolarItemType.Planet), sunNode);
            Node<SolarItem> mondNode = new(new SolarItem("Mond", SolarItemType.Trabant), earthNode);
            Node<SolarItem> mondUnterNode = new(new SolarItem("Mond", SolarItemType.Trabant), mondNode);

            Node<SolarItem> marsNode = new Node<SolarItem>(new SolarItem("Mars", SolarItemType.Planet), sunNode);
            Node<SolarItem> phobosNode = new Node<SolarItem>(new SolarItem("Phobos", SolarItemType.Trabant), marsNode);
            Node<SolarItem> deimosNode = new Node<SolarItem>(new SolarItem("Deimos", SolarItemType.Trabant), marsNode);

            Node<SolarItem> jupiterNode = new Node<SolarItem>(new SolarItem("Jupiter", SolarItemType.Planet), sunNode);
            Node<SolarItem> europaNode = new Node<SolarItem>(new SolarItem("Europa", SolarItemType.Trabant), jupiterNode);
            Node<SolarItem> ioNode = new Node<SolarItem>(new SolarItem("Io", SolarItemType.Trabant), jupiterNode);
            Node<SolarItem> ganymedNode = new Node<SolarItem>(new SolarItem("Ganymed", SolarItemType.Trabant), jupiterNode);
            Node<SolarItem> kallistoNode = new Node<SolarItem>(new SolarItem("Kallisto", SolarItemType.Trabant), jupiterNode);
            Node<SolarItem> metisNode = new Node<SolarItem>(new SolarItem("Metis", SolarItemType.Trabant), jupiterNode);
            Node<SolarItem> adrasteaNode = new Node<SolarItem>(new SolarItem("Adrastea", SolarItemType.Trabant), jupiterNode);
            Node<SolarItem> amaltheaNode = new Node<SolarItem>(new SolarItem("Amalthea", SolarItemType.Trabant), jupiterNode);
            Node<SolarItem> thebeNode = new Node<SolarItem>(new SolarItem("Thebe", SolarItemType.Trabant), jupiterNode);

            Node<SolarItem> saturnNode = new Node<SolarItem>(new SolarItem("Saturn", SolarItemType.Planet), sunNode);
            Node<SolarItem> titanNode = new Node<SolarItem>(new SolarItem("Titan", SolarItemType.Trabant), saturnNode);
            Node<SolarItem> rheaNode = new Node<SolarItem>(new SolarItem("Rhea", SolarItemType.Trabant), saturnNode);
            Node<SolarItem> dioneNode = new Node<SolarItem>(new SolarItem("Dione", SolarItemType.Trabant), saturnNode);
            Node<SolarItem> tethysNode = new Node<SolarItem>(new SolarItem("Tethys", SolarItemType.Trabant), saturnNode);
            Node<SolarItem> japetusNode = new Node<SolarItem>(new SolarItem("Japetus", SolarItemType.Trabant), saturnNode);
            Node<SolarItem> telestoNode = new Node<SolarItem>(new SolarItem("Telesto", SolarItemType.Trabant), saturnNode);
            Node<SolarItem> calypsoNode = new Node<SolarItem>(new SolarItem("Calypso", SolarItemType.Trabant), saturnNode);

            Node<SolarItem> uranusNode = new Node<SolarItem>(new SolarItem("Uranus", SolarItemType.Planet), sunNode);
            Node<SolarItem> mirandaNode = new Node<SolarItem>(new SolarItem("Miranda", SolarItemType.Trabant), uranusNode);
            Node<SolarItem> arielNode = new Node<SolarItem>(new SolarItem("Ariel", SolarItemType.Trabant), uranusNode);
            Node<SolarItem> umbrielNode = new Node<SolarItem>(new SolarItem("Umbriel", SolarItemType.Trabant), uranusNode);
            Node<SolarItem> titaniaNode = new Node<SolarItem>(new SolarItem("Titania", SolarItemType.Trabant), uranusNode);
            Node<SolarItem> oberonNode = new Node<SolarItem>(new SolarItem("Oberon", SolarItemType.Trabant), uranusNode);
            Node<SolarItem> tritonNode = new Node<SolarItem>(new SolarItem("Triton", SolarItemType.Trabant), uranusNode);

            Node<SolarItem> neptunNode = new Node<SolarItem>(new SolarItem("Neptun", SolarItemType.Planet), sunNode);
            Node<SolarItem> proteusNode = new Node<SolarItem>(new SolarItem("Proteus", SolarItemType.Trabant), neptunNode);
            Node<SolarItem> halimedeNode = new Node<SolarItem>(new SolarItem("Halimede", SolarItemType.Trabant), neptunNode);
            Node<SolarItem> nereidNode = new Node<SolarItem>(new SolarItem("Nereid", SolarItemType.Trabant), neptunNode);
            Node<SolarItem> naiadNode = new Node<SolarItem>(new SolarItem("Naiad", SolarItemType.Trabant), neptunNode);
            Node<SolarItem> thalasaaNode = new Node<SolarItem>(new SolarItem("Thalasaa", SolarItemType.Trabant), neptunNode);

            DisplaySolarSystemDepth(sunNode);

            void DisplaySolarSystem(Node<SolarItem> solarNode)
            {
                if (solarNode.Item.Type == SolarItemType.Star)
                    Console.WriteLine($"Sonne: {solarNode.Item.Description}");

                if (solarNode.Item.Type == SolarItemType.Planet)
                    Console.WriteLine($"\tPlanet: {solarNode.Item.Description} - kreist um {solarNode.ParentNode.Item.Description}");

                if (solarNode.Item.Type == SolarItemType.Trabant)
                    Console.WriteLine($"\t\t -Mond: {solarNode.Item.Description} - kreist um {solarNode.ParentNode.Item.Description}");

                if (solarNode.Item.Type != SolarItemType.Trabant)
                {
                    foreach (Node<SolarItem> node in solarNode.Childrens)
                    {
                        DisplaySolarSystem(node);
                    }
                }
            }

            void DisplaySolarSystemDepth(Node<SolarItem> solarNode, int depth = 0) //depth wird an die Children weitergegeben
            {
                if (depth == 0)
                    Console.WriteLine($"{solarNode.Item.Type}: {solarNode.Item.Description}");

                foreach (Node<SolarItem> node in solarNode.Childrens)
                {
                    string kreistUm = solarNode.ParentNode != null ? $" - kreist um {solarNode.ParentNode.Item.Description}" : "";
                    depth++;
                    Console.WriteLine($"{new string('\t', depth)} {node.Item.Type}: {node.Item.Description}{kreistUm}");
                    DisplaySolarSystemDepth(node, depth);
                    depth--;
                }
            }
        }
    }
}
