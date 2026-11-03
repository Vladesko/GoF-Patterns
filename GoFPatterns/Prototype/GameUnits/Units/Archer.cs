namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Prototipo concreto. Cada clase sabe cómo copiarse a sí misma, incluidos sus propios campos.
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

        public override string ToString() => $"{base.ToString()}, flechas: {Arrows}";
    }
}
