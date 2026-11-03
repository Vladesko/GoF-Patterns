namespace GoFPatterns.Flyweight.Forest
{
    public class DemonstrateForest
    {
        private const int TreesCount = 100_000;

        public void Demonstrate()
        {
            Console.WriteLine("=== Flyweight: Forest ===\n");

            (string Name, string Color, string Texture)[] kinds =
            [
                ("Oak", "dark green", "oak.png"),
                ("Pine", "green", "pine.png"),
                ("Birch", "light green", "birch.png"),
                ("Maple", "red", "maple_autumn.png")
            ];

            var forest = new ForestMap();
            var random = new Random(42);
            for (var i = 0; i < TreesCount; i++)
            {
                var kind = kinds[random.Next(kinds.Length)];
                forest.PlantTree(random.Next(10_000), random.Next(10_000), kind.Name, kind.Color, kind.Texture);
            }

            Console.WriteLine("\nFirst trees on the map:");
            foreach (var line in forest.Draw(3))
            {
                Console.WriteLine($"  {line}");
            }

            // Rough memory estimate: tree = 2 ints + reference (~ 16 bytes of data).
            const long treeContextSize = 16;
            var withoutFlyweight = forest.TreesCount * (TreeType.TextureSize + treeContextSize);
            var withFlyweight = forest.TreeTypesCount * TreeType.TextureSize + forest.TreesCount * treeContextSize;

            Console.WriteLine($"\nTrees: {forest.TreesCount:N0}, tree type objects: {forest.TreeTypesCount}");
            Console.WriteLine($"Memory without flyweight: ~{withoutFlyweight / 1024d / 1024 / 1024:N1} GB");
            Console.WriteLine($"Memory with flyweight:    ~{withFlyweight / 1024d / 1024:N1} MB");
        }
    }
}
