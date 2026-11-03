namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Klasse des Subsystems.
    public class SoundSystem
    {
        public void On() => Console.WriteLine("  Soundsystem: an");
        public void Off() => Console.WriteLine("  Soundsystem: aus");
        public void SetSurroundSound() => Console.WriteLine("  Soundsystem: Surround 5.1");
        public void SetVolume(int level) => Console.WriteLine($"  Soundsystem: Lautstärke {level}");
    }
}
