namespace GoFPatterns.Prototype.GameUnits
{
    // Client - genera unità clonando un prototype. Non sa quale unità concreta sta copiando.
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
