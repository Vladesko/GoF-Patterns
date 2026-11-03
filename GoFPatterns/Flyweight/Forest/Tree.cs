namespace GoFPatterns.Flyweight.Forest
{
    // Contexto: contiene el estado extrínseco (único) y una referencia al flyweight.
    // Es muy pequeño: dos ints y una referencia.
    public class Tree(int x, int y, TreeType type)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public TreeType Type { get; } = type;

        public string Draw() => Type.Draw(X, Y);
    }
}
