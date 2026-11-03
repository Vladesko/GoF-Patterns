namespace GoFPatterns.Iterator.Tree
{
    public class DemonstrateTree
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Iterator: Baumdurchlauf ===\n");

            var catalog = new CategoryNode("Katalog").Add(
                new CategoryNode("Elektronik").Add(
                    new CategoryNode("Smartphones"),
                    new CategoryNode("Laptops").Add(
                        new CategoryNode("Gaming"),
                        new CategoryNode("Ultrabooks"))),
                new CategoryNode("Bücher").Add(
                    new CategoryNode("Belletristik"),
                    new CategoryNode("Wissenschaft")));

            Console.WriteLine("Tiefensuche (wie ein Menü):");
            foreach (var (node, depth) in catalog.DepthFirst())
            {
                Console.WriteLine($"  {new string(' ', depth * 2)}{node.Name}");
            }

            Console.WriteLine("\nBreitensuche (Ebene für Ebene):");
            foreach (var (node, depth) in catalog.BreadthFirst())
            {
                Console.WriteLine($"  Ebene {depth}: {node.Name}");
            }

            // Iteratoren sind lazy: Der Durchlauf endet, sobald das Gesuchte gefunden ist.
            var visited = 0;
            var found = catalog.DepthFirst()
                .Select(x => { visited++; return x.Node; })
                .First(n => n.Name == "Laptops");
            Console.WriteLine($"\n'{found.Name}' gefunden nach dem Besuch von {visited} von {catalog.DepthFirst().Count()} Knoten.");
        }
    }
}
