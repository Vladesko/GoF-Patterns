namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Prototipo concreto con un campo de referencia: la lista de hechizos también se copia (copia profunda).
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

        public override string ToString() => $"{base.ToString()}, hechizos: {string.Join("/", Spells)}";
    }
}
