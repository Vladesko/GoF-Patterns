namespace GoFPatterns.Prototype.GameUnits
{
    // Client - spawns units by cloning a prototype. It doesn't know which concrete unit it copies.
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
