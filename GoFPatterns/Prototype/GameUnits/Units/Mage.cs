namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Конкретный прототип со ссылочным полем - список заклинаний тоже копируется (глубокая копия).
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

        public override string ToString() => $"{base.ToString()}, заклинания: {string.Join("/", Spells)}";
    }
}
