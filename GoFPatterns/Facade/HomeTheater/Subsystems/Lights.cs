namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Класс подсистемы.
    public class Lights
    {
        public void Dim(int percent) => Console.WriteLine($"  Свет: приглушён до {percent}%");
        public void On() => Console.WriteLine("  Свет: включён");
    }
}
