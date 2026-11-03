namespace GoFPatterns.Prototype.GameUnits
{
    // Client – erzeugt Einheiten durch Klonen eines Prototyps. Er weiß nicht, welche konkrete Einheit er kopiert.
    public class Spawner(Unit prototype)
    {
        private int _counter;

        public Unit Spawn(Position position)
        {
            var unit = prototype.Clone();
            unit.Name = $"{prototype.Name} #{++_counter}";
            unit.Position = position;
            return unit;
        }
    }
}
