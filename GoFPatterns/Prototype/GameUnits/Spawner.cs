namespace GoFPatterns.Prototype.GameUnits
{
    // Клиент - создаёт юниты, клонируя прототип. Он не знает, какой конкретный юнит копирует.
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
