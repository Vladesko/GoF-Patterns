namespace GoFPatterns.Iterator.Tree
{
    public class DemonstrateTree
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Iterator: Attraversamento di un albero ===\n");

            var catalog = new CategoryNode("Catalogo").Add(
                new CategoryNode("Elettronica").Add(
                    new CategoryNode("Telefoni"),
                    new CategoryNode("Portatili").Add(
                        new CategoryNode("Gaming"),
                        new CategoryNode("Ultrabooks"))),
                new CategoryNode("Libri").Add(
                    new CategoryNode("Narrativa"),
                    new CategoryNode("Scienza")));

            Console.WriteLine("In profondità (come un menu):");
            foreach (var (node, depth) in catalog.DepthFirst())
            {
                Console.WriteLine($"  {new string(' ', depth * 2)}{node.Name}");
            }

            Console.WriteLine("\nIn ampiezza (livello per livello):");
            foreach (var (node, depth) in catalog.BreadthFirst())
            {
                Console.WriteLine($"  livello {depth}: {node.Name}");
            }

            // Gli iterator sono lazy: l'attraversamento si ferma appena troviamo ciò che ci serve.
            var visited = 0;
            var found = catalog.DepthFirst()
                .Select(x => { visited++; return x.Node; })
                .First(n => n.Name == "Portatili");
            Console.WriteLine($"\nTrovato '{found.Name}' dopo aver visitato {visited} nodi su {catalog.DepthFirst().Count()}.");
        }
    }
}
