namespace GoFPatterns.Flyweight.Forest
{
    // Flyweight factory - returns existing flyweight or creates a new one.
    // Client never creates TreeType directly, so duplicates are impossible.
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
                Console.WriteLine($"  [Factory] created new tree type: {name}, {color}");
            }
            return type;
        }
    }
}
