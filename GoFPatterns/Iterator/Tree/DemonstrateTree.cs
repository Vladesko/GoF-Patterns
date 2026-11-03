namespace GoFPatterns.Iterator.Tree
{
    public class DemonstrateTree
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Итератор: обход дерева ===\n");

            var catalog = new CategoryNode("Каталог").Add(
                new CategoryNode("Электроника").Add(
                    new CategoryNode("Телефоны"),
                    new CategoryNode("Ноутбуки").Add(
                        new CategoryNode("Игровые"),
                        new CategoryNode("Ультрабуки"))),
                new CategoryNode("Книги").Add(
                    new CategoryNode("Художественная литература"),
                    new CategoryNode("Наука")));

            Console.WriteLine("В глубину (как меню):");
            foreach (var (node, depth) in catalog.DepthFirst())
            {
                Console.WriteLine($"  {new string(' ', depth * 2)}{node.Name}");
            }

            Console.WriteLine("\nВ ширину (уровень за уровнем):");
            foreach (var (node, depth) in catalog.BreadthFirst())
            {
                Console.WriteLine($"  уровень {depth}: {node.Name}");
            }

            // Итераторы ленивые: обход останавливается, как только мы нашли то, что нужно.
            var visited = 0;
            var found = catalog.DepthFirst()
                .Select(x => { visited++; return x.Node; })
                .First(n => n.Name == "Ноутбуки");
            Console.WriteLine($"\nНайдено '{found.Name}' после посещения {visited} из {catalog.DepthFirst().Count()} узлов.");
        }
    }
}
