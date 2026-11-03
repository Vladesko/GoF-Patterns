namespace GoFPatterns.Flyweight.Forest
{
    // Flyweight - contiene lo stato intrinseco (condiviso): nome, colore e texture pesante.
    // È immutabile, quindi un solo oggetto può essere usato in sicurezza da migliaia di alberi.
    public sealed class TreeType(string name, string color, string texturePath)
    {
        // Emulazione di dati pesanti - texture da 1 MB.
        private readonly byte[] _texture = new byte[1024 * 1024];

        public string Name { get; } = name;
        public string Color { get; } = color;
        public string TexturePath { get; } = texturePath;

        public static long TextureSize => 1024 * 1024;

        // Lo stato estrinseco (unico) - le coordinate - viene passato come parametri.
        public string Draw(int x, int y) => $"{Name} ({Color}) in [{x}, {y}] con {TexturePath} ({_texture.Length / 1024} KB)";
    }
}
