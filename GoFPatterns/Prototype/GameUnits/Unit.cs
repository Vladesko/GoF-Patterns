namespace GoFPatterns.Prototype.GameUnits
{
    // Prototype - unità astratta con il metodo Clone.
    public abstract class Unit
    {
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public Position Position { get; set; } = new(0, 0);

        public abstract Unit Clone();

        public override string ToString() => $"{GetType().Name} '{Name}' HP:{Health} in {Position}";
    }

    // Un record è immutabile, quindi può essere condiviso in sicurezza tra i cloni.
    // L'espressione "with" è il modo integrato di C# per clonare un record con modifiche.
    public record Position(int X, int Y);
}
