namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Konkreter Prototyp mit Referenzfeld – auch die Liste der Zauber wird kopiert (tiefe Kopie).
    public class Mage : Unit
    {
        public List<string> Spells { get; set; } = [];

        public override Unit Clone() => new Mage
        {
            Name = Name,
            Health = Health,
            Position = Position,
            Spells = [.. Spells]
        };

        public override string ToString() => $"{base.ToString()}, Zauber: {string.Join("/", Spells)}";
    }
}
