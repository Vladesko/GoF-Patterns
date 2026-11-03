namespace GoFPatterns.Iterator.Tree
{
    public class DemonstrateTree
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Iterator: Tree traversal ===\n");

            var catalog = new CategoryNode("Catalog").Add(
                new CategoryNode("Electronics").Add(
                    new CategoryNode("Phones"),
                    new CategoryNode("Laptops").Add(
                        new CategoryNode("Gaming"),
                        new CategoryNode("Ultrabooks"))),
                new CategoryNode("Books").Add(
                    new CategoryNode("Fiction"),
                    new CategoryNode("Science")));

            Console.WriteLine("Depth-first (as a menu):");
            foreach (var (node, depth) in catalog.DepthFirst())
            {
                Console.WriteLine($"  {new string(' ', depth * 2)}{node.Name}");
            }

            Console.WriteLine("\nBreadth-first (level by level):");
            foreach (var (node, depth) in catalog.BreadthFirst())
            {
                Console.WriteLine($"  level {depth}: {node.Name}");
            }

            // Iterators are lazy: traversal stops as soon as we have found what we need.
            var visited = 0;
            var found = catalog.DepthFirst()
                .Select(x => { visited++; return x.Node; })
                .First(n => n.Name == "Laptops");
            Console.WriteLine($"\nFound '{found.Name}' after visiting {visited} of {catalog.DepthFirst().Count()} nodes.");
        }
    }
}
