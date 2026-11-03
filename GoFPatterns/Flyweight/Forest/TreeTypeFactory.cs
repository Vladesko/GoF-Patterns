namespace GoFPatterns.Flyweight.Forest
{
    // Fábrica de flyweights: devuelve un flyweight existente o crea uno nuevo.
    // El cliente nunca crea TreeType directamente, así que los duplicados son imposibles.
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
                Console.WriteLine($"  [Factory] creado un nuevo tipo de árbol: {name}, {color}");
            }
            return type;
        }
    }
}
