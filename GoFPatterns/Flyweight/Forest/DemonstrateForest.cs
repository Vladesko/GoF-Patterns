namespace GoFPatterns.Flyweight.Forest
{
    public class DemonstrateForest
    {
        private const int TreesCount = 100_000;

        public void Demonstrate()
        {
            Console.WriteLine("=== Flyweight: Foresta ===\n");

            (string Name, string Color, string Texture)[] kinds =
            [
                ("Quercia", "verde scuro", "oak.png"),
                ("Pino", "verde", "pine.png"),
                ("Betulla", "verde chiaro", "birch.png"),
                ("Acero", "rosso", "maple_autumn.png")
            ];

            var forest = new ForestMap();
            var random = new Random(42);
            for (var i = 0; i < TreesCount; i++)
            {
                var kind = kinds[random.Next(kinds.Length)];
                forest.PlantTree(random.Next(10_000), random.Next(10_000), kind.Name, kind.Color, kind.Texture);
            }

            Console.WriteLine("\nPrimi alberi sulla mappa:");
            foreach (var line in forest.Draw(3))
            {
                Console.WriteLine($"  {line}");
            }

            // Stima approssimativa della memoria: albero = 2 int + riferimento (~ 16 byte di dati).
            const long treeContextSize = 16;
            var withoutFlyweight = forest.TreesCount * (TreeType.TextureSize + treeContextSize);
            var withFlyweight = forest.TreeTypesCount * TreeType.TextureSize + forest.TreesCount * treeContextSize;

            Console.WriteLine($"\nAlberi: {forest.TreesCount:N0}, oggetti tipo di albero: {forest.TreeTypesCount}");
            Console.WriteLine($"Memoria senza flyweight: ~{withoutFlyweight / 1024d / 1024 / 1024:N1} GB");
            Console.WriteLine($"Memoria con flyweight:   ~{withFlyweight / 1024d / 1024:N1} MB");
        }
    }
}
