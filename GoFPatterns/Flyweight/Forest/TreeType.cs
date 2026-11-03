namespace GoFPatterns.Flyweight.Forest
{
    // Flyweight - contains intrinsic (shared) state: name, color and heavy texture.
    // It is immutable, so one object can be safely used by thousands of trees.
    public sealed class TreeType(string name, string color, string texturePath)
    {
        // Emulation of heavy data - texture of 1 MB.
        private readonly byte[] _texture = new byte[1024 * 1024];

        public string Name { get; } = name;
        public string Color { get; } = color;
        public string TexturePath { get; } = texturePath;

        public static long TextureSize => 1024 * 1024;

        // Extrinsic (unique) state - coordinates - is passed as parameters.
        public string Draw(int x, int y) => $"{Name} ({Color}) at [{x}, {y}] using {TexturePath} ({_texture.Length / 1024} KB)";
    }
}
