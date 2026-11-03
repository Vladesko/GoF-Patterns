namespace GoFPatterns.Flyweight.Forest
{
    // Context - contains extrinsic (unique) state and a reference to flyweight.
    // It is very small: two ints and one reference.
    public class Tree(int x, int y, TreeType type)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public TreeType Type { get; } = type;

        public string Draw() => Type.Draw(X, Y);
    }
}
