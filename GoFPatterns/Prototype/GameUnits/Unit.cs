namespace GoFPatterns.Prototype.GameUnits
{
    // Prototyp – abstrakte Einheit mit der Methode Clone.
    public abstract class Unit
    {
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public Position Position { get; set; } = new(0, 0);

        public abstract Unit Clone();

        public override string ToString() => $"{GetType().Name} '{Name}' HP:{Health} bei {Position}";
    }

    // Ein Record ist unveränderlich und kann daher gefahrlos von mehreren Klonen geteilt werden.
    // Der Ausdruck "with" ist der eingebaute C#-Weg, einen Record mit Änderungen zu klonen.
    public record Position(int X, int Y);
}
