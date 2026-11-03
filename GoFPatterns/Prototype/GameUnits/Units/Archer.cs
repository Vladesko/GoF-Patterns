namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Concrete prototype. Each class knows how to copy itself including its own fields.
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

        public override string ToString() => $"{base.ToString()}, arrows: {Arrows}";
    }
}
