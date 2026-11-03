namespace GoFPatterns.Flyweight.Forest
{
    // Flyweight-Fabrik – liefert ein vorhandenes Flyweight oder erzeugt ein neues.
    // Der Client erzeugt TreeType nie direkt, daher sind Duplikate ausgeschlossen.
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
                Console.WriteLine($"  [Factory] neuer Baumtyp erstellt: {name}, {color}");
            }
            return type;
        }
    }
}
