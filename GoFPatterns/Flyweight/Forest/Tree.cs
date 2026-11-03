namespace GoFPatterns.Flyweight.Forest
{
    // Kontext – enthält den extrinsischen (einzigartigen) Zustand und eine Referenz auf das Flyweight.
    // Er ist sehr klein: zwei ints und eine Referenz.
    public class Tree(int x, int y, TreeType type)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public TreeType Type { get; } = type;

        public string Draw() => Type.Draw(X, Y);
    }
}
