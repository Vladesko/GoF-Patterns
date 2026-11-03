namespace GoFPatterns.Iterator.Tree
{
    public class DemonstrateTree
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Iterator: recorrido de árbol ===\n");

            var catalog = new CategoryNode("Catálogo").Add(
                new CategoryNode("Electrónica").Add(
                    new CategoryNode("Teléfonos"),
                    new CategoryNode("Portátiles").Add(
                        new CategoryNode("Gaming"),
                        new CategoryNode("Ultrabooks"))),
                new CategoryNode("Libros").Add(
                    new CategoryNode("Ficción"),
                    new CategoryNode("Ciencia")));

            Console.WriteLine("En profundidad (como un menú):");
            foreach (var (node, depth) in catalog.DepthFirst())
            {
                Console.WriteLine($"  {new string(' ', depth * 2)}{node.Name}");
            }

            Console.WriteLine("\nEn anchura (nivel a nivel):");
            foreach (var (node, depth) in catalog.BreadthFirst())
            {
                Console.WriteLine($"  nivel {depth}: {node.Name}");
            }

            // Los iteradores son perezosos: el recorrido se detiene en cuanto encontramos lo que necesitamos.
            var visited = 0;
            var found = catalog.DepthFirst()
                .Select(x => { visited++; return x.Node; })
                .First(n => n.Name == "Portátiles");
            Console.WriteLine($"\nEncontrado '{found.Name}' tras visitar {visited} de {catalog.DepthFirst().Count()} nodos.");
        }
    }
}
