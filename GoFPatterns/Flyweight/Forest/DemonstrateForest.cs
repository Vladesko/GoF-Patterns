namespace GoFPatterns.Flyweight.Forest
{
    public class DemonstrateForest
    {
        private const int TreesCount = 100_000;

        public void Demonstrate()
        {
            Console.WriteLine("=== Легковес: лес ===\n");

            (string Name, string Color, string Texture)[] kinds =
            [
                ("Дуб", "тёмно-зелёный", "oak.png"),
                ("Сосна", "зелёный", "pine.png"),
                ("Берёза", "светло-зелёный", "birch.png"),
                ("Клён", "красный", "maple_autumn.png")
            ];

            var forest = new ForestMap();
            var random = new Random(42);
            for (var i = 0; i < TreesCount; i++)
            {
                var kind = kinds[random.Next(kinds.Length)];
                forest.PlantTree(random.Next(10_000), random.Next(10_000), kind.Name, kind.Color, kind.Texture);
            }

            Console.WriteLine("\nПервые деревья на карте:");
            foreach (var line in forest.Draw(3))
            {
                Console.WriteLine($"  {line}");
            }

            // Грубая оценка памяти: дерево = 2 int + ссылка (~ 16 байт данных).
            const long treeContextSize = 16;
            var withoutFlyweight = forest.TreesCount * (TreeType.TextureSize + treeContextSize);
            var withFlyweight = forest.TreeTypesCount * TreeType.TextureSize + forest.TreesCount * treeContextSize;

            Console.WriteLine($"\nДеревьев: {forest.TreesCount:N0}, объектов типов деревьев: {forest.TreeTypesCount}");
            Console.WriteLine($"Память без легковеса: ~{withoutFlyweight / 1024d / 1024 / 1024:N1} ГБ");
            Console.WriteLine($"Память с легковесом:  ~{withFlyweight / 1024d / 1024:N1} МБ");
        }
    }
}
