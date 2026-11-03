namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Класс подсистемы - ничего не знает о фасаде.
    public class Projector
    {
        public void On() => Console.WriteLine("  Проектор: включён");
        public void Off() => Console.WriteLine("  Проектор: выключен");
        public void SetInput(string input) => Console.WriteLine($"  Проектор: вход {input}");
        public void WideScreenMode() => Console.WriteLine("  Проектор: режим 16:9");
    }
}
