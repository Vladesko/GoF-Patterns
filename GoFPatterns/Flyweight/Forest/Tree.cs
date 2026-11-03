namespace GoFPatterns.Flyweight.Forest
{
    // Context - contiene lo stato estrinseco (unico) e un riferimento al flyweight.
    // È molto piccolo: due int e un riferimento.
    public class Tree(int x, int y, TreeType type)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public TreeType Type { get; } = type;

        public string Draw() => Type.Draw(X, Y);
    }
}
