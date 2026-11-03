namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Concrete prototype with reference field - list of spells is copied too (deep copy).
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

        public override string ToString() => $"{base.ToString()}, spells: {string.Join("/", Spells)}";
    }
}
