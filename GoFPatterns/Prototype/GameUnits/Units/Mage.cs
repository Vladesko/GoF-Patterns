namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Prototype concreto con un campo di tipo riferimento - anche la lista degli incantesimi viene copiata (copia profonda).
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

        public override string ToString() => $"{base.ToString()}, incantesimi: {string.Join("/", Spells)}";
    }
}
