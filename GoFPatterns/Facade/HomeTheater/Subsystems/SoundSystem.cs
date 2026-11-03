namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Класс подсистемы.
    public class SoundSystem
    {
        public void On() => Console.WriteLine("  Аудиосистема: включена");
        public void Off() => Console.WriteLine("  Аудиосистема: выключена");
        public void SetSurroundSound() => Console.WriteLine("  Аудиосистема: объёмный звук 5.1");
        public void SetVolume(int level) => Console.WriteLine($"  Аудиосистема: громкость {level}");
    }
}
