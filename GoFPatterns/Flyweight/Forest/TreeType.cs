namespace GoFPatterns.Flyweight.Forest
{
    // Flyweight – enthält den intrinsischen (gemeinsamen) Zustand: Name, Farbe und schwere Textur.
    // Es ist unveränderlich, daher kann ein Objekt gefahrlos von Tausenden Bäumen genutzt werden.
    public sealed class TreeType(string name, string color, string texturePath)
    {
        // Emulation schwerer Daten – eine Textur von 1 MB.
        private readonly byte[] _texture = new byte[1024 * 1024];

        public string Name { get; } = name;
        public string Color { get; } = color;
        public string TexturePath { get; } = texturePath;

        public static long TextureSize => 1024 * 1024;

        // Der extrinsische (einzigartige) Zustand – die Koordinaten – wird als Parameter übergeben.
        public string Draw(int x, int y) => $"{Name} ({Color}) bei [{x}, {y}] mit {TexturePath} ({_texture.Length / 1024} KB)";
    }
}
