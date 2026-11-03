namespace GoFPatterns.Flyweight.Forest
{
    // Контекст - содержит внешнее (уникальное) состояние и ссылку на легковес.
    // Он очень маленький: два int и одна ссылка.
    public class Tree(int x, int y, TreeType type)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public TreeType Type { get; } = type;

        public string Draw() => Type.Draw(X, Y);
    }
}
