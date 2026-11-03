namespace GoFPatterns.Flyweight.Forest
{
    // Фабрика легковесов - возвращает существующий легковес или создаёт новый.
    // Клиент никогда не создаёт TreeType напрямую, поэтому дубликаты невозможны.
    public class TreeTypeFactory
    {
        private readonly Dictionary<string, TreeType> _types = [];

        public int Count => _types.Count;

        public TreeType GetTreeType(string name, string color, string texturePath)
        {
            var key = $"{name}|{color}|{texturePath}";
            if (!_types.TryGetValue(key, out var type))
            {
                type = new TreeType(name, color, texturePath);
                _types[key] = type;
                Console.WriteLine($"  [Фабрика] создан новый тип дерева: {name}, {color}");
            }
            return type;
        }
    }
}
