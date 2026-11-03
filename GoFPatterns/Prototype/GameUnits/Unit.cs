namespace GoFPatterns.Prototype.GameUnits
{
    // Prototype: unidad abstracta con el método Clone.
    public abstract class Unit
    {
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public Position Position { get; set; } = new(0, 0);

        public abstract Unit Clone();

        public override string ToString() => $"{GetType().Name} '{Name}' HP:{Health} en {Position}";
    }

    // Un record es inmutable, así que se puede compartir de forma segura entre clones.
    // La expresión "with" es la forma integrada en C# de clonar un record con cambios.
    public record Position(int X, int Y);
}
