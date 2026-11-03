namespace GoFPatterns.Prototype.GameUnits
{
    // Cliente: genera unidades clonando un prototipo. No sabe qué unidad concreta está copiando.
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
