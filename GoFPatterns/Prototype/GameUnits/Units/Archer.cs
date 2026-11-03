namespace GoFPatterns.Prototype.GameUnits.Units
{
    // Конкретный прототип. Каждый класс знает, как скопировать себя, включая собственные поля.
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

        public override string ToString() => $"{base.ToString()}, стрел: {Arrows}";
    }
}
