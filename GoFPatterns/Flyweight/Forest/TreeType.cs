namespace GoFPatterns.Flyweight.Forest
{
    // Flyweight: contiene el estado intrínseco (compartido): nombre, color y textura pesada.
    // Es inmutable, así que un mismo objeto puede ser usado de forma segura por miles de árboles.
    public sealed class TreeType(string name, string color, string texturePath)
    {
        // Emulación de datos pesados: textura de 1 MB.
        private readonly byte[] _texture = new byte[1024 * 1024];

        public string Name { get; } = name;
        public string Color { get; } = color;
        public string TexturePath { get; } = texturePath;

        public static long TextureSize => 1024 * 1024;

        // El estado extrínseco (único), las coordenadas, se pasa como parámetros.
        public string Draw(int x, int y) => $"{Name} ({Color}) en [{x}, {y}] usando {TexturePath} ({_texture.Length / 1024} KB)";
    }
}
