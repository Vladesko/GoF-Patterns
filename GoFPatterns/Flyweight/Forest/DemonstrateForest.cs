namespace GoFPatterns.Flyweight.Forest
{
    public class DemonstrateForest
    {
        private const int TreesCount = 100_000;

        public void Demonstrate()
        {
            Console.WriteLine("=== Flyweight: Wald ===\n");

            (string Name, string Color, string Texture)[] kinds =
            [
                ("Eiche", "dunkelgrün", "oak.png"),
                ("Kiefer", "grün", "pine.png"),
                ("Birke", "hellgrün", "birch.png"),
                ("Ahorn", "rot", "maple_autumn.png")
            ];

            var forest = new ForestMap();
            var random = new Random(42);
            for (var i = 0; i < TreesCount; i++)
            {
                var kind = kinds[random.Next(kinds.Length)];
                forest.PlantTree(random.Next(10_000), random.Next(10_000), kind.Name, kind.Color, kind.Texture);
            }

            Console.WriteLine("\nDie ersten Bäume auf der Karte:");
            foreach (var line in forest.Draw(3))
            {
                Console.WriteLine($"  {line}");
            }

            // Grobe Speicherabschätzung: Baum = 2 ints + Referenz (~ 16 Byte Daten).
            const long treeContextSize = 16;
            var withoutFlyweight = forest.TreesCount * (TreeType.TextureSize + treeContextSize);
            var withFlyweight = forest.TreeTypesCount * TreeType.TextureSize + forest.TreesCount * treeContextSize;

            Console.WriteLine($"\nBäume: {forest.TreesCount:N0}, Baumtyp-Objekte: {forest.TreeTypesCount}");
            Console.WriteLine($"Speicher ohne Flyweight: ~{withoutFlyweight / 1024d / 1024 / 1024:N1} GB");
            Console.WriteLine($"Speicher mit Flyweight:  ~{withFlyweight / 1024d / 1024:N1} MB");
        }
    }
}
