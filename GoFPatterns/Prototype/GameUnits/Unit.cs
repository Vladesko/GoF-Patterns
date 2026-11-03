namespace GoFPatterns.Prototype.GameUnits
{
    // Prototype - abstract unit with Clone method.
    public abstract class Unit
    {
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public Position Position { get; set; } = new(0, 0);

        public abstract Unit Clone();

        public override string ToString() => $"{GetType().Name} '{Name}' HP:{Health} at {Position}";
    }

    // Record is immutable, so it can be safely shared between clones.
    // "with" expression is a built-in C# way to clone record with changes.
    public record Position(int X, int Y);
}
