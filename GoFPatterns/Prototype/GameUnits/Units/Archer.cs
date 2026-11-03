namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Prototype concreto. Ogni classe sa come copiare se stessa, inclusi i propri campi.
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

        public override string ToString() => $"{base.ToString()}, frecce: {Arrows}";
    }
}
