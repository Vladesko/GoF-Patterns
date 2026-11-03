namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Konkreter Prototyp. Jede Klasse weiß, wie sie sich samt eigener Felder kopiert.
    public class Archer : Unit
    {
        public int Arrows { get; set; }

        public override Unit Clone() => new Archer
        {
            Name = Name,
            Health = Health,
            Position = Position,
            Arrows = Arrows
        };

        public override string ToString() => $"{base.ToString()}, Pfeile: {Arrows}";
    }
}
