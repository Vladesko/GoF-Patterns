namespace GoFPatterns.Prototype.GameUnits
{
    // Прототип - абстрактный юнит с методом Clone.
    public abstract class Unit
    {
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public Position Position { get; set; } = new(0, 0);

        public abstract Unit Clone();

        public override string ToString() => $"{GetType().Name} '{Name}' HP:{Health} в точке {Position}";
    }

    // Record неизменяем, поэтому его можно безопасно разделять между клонами.
    // Выражение "with" - встроенный в C# способ клонировать record с изменениями.
    public record Position(int X, int Y);
}
