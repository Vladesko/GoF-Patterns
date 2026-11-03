namespace GoFPatterns.Flyweight.Forest
{
    public class DemonstrateForest
    {
        private const int TreesCount = 100_000;

        public void Demonstrate()
        {
            Console.WriteLine("=== Flyweight: bosque ===\n");

            (string Name, string Color, string Texture)[] kinds =
            [
                ("Roble", "verde oscuro", "oak.png"),
                ("Pino", "verde", "pine.png"),
                ("Abedul", "verde claro", "birch.png"),
                ("Arce", "rojo", "maple_autumn.png")
            ];

            var forest = new ForestMap();
            var random = new Random(42);
            for (var i = 0; i < TreesCount; i++)
            {
                var kind = kinds[random.Next(kinds.Length)];
                forest.PlantTree(random.Next(10_000), random.Next(10_000), kind.Name, kind.Color, kind.Texture);
            }

            Console.WriteLine("\nPrimeros árboles del mapa:");
            foreach (var line in forest.Draw(3))
            {
                Console.WriteLine($"  {line}");
            }

            // Estimación aproximada de memoria: árbol = 2 ints + referencia (~ 16 bytes de datos).
            const long treeContextSize = 16;
            var withoutFlyweight = forest.TreesCount * (TreeType.TextureSize + treeContextSize);
            var withFlyweight = forest.TreeTypesCount * TreeType.TextureSize + forest.TreesCount * treeContextSize;

            Console.WriteLine($"\nÁrboles: {forest.TreesCount:N0}, objetos de tipo de árbol: {forest.TreeTypesCount}");
            Console.WriteLine($"Memoria sin flyweight: ~{withoutFlyweight / 1024d / 1024 / 1024:N1} GB");
            Console.WriteLine($"Memoria con flyweight: ~{withFlyweight / 1024d / 1024:N1} MB");
        }
    }
}
