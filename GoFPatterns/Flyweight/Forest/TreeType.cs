namespace GoFPatterns.Flyweight.Forest
{
    // Легковес - содержит внутреннее (разделяемое) состояние: название, цвет и тяжёлую текстуру.
    // Он неизменяем, поэтому один объект можно безопасно использовать в тысячах деревьев.
    public sealed class TreeType(string name, string color, string texturePath)
    {
        // Эмуляция тяжёлых данных - текстура размером 1 МБ.
        private readonly byte[] _texture = new byte[1024 * 1024];

        public string Name { get; } = name;
        public string Color { get; } = color;
        public string TexturePath { get; } = texturePath;

        public static long TextureSize => 1024 * 1024;

        // Внешнее (уникальное) состояние - координаты - передаётся через параметры.
        public string Draw(int x, int y) => $"{Name} ({Color}) в точке [{x}, {y}], текстура {TexturePath} ({_texture.Length / 1024} КБ)";
    }
}
