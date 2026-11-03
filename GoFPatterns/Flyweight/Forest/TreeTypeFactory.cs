namespace GoFPatterns.Flyweight.Forest
{
    // Flyweight factory - restituisce un flyweight esistente o ne crea uno nuovo.
    // Il client non crea mai TreeType direttamente, quindi i duplicati sono impossibili.
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
                Console.WriteLine($"  [Factory] creato un nuovo tipo di albero: {name}, {color}");
            }
            return type;
        }
    }
}
