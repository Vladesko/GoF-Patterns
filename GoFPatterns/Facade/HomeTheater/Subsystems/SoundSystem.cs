namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Subsystem class.
    public class SoundSystem
    {
        public void On() => Console.WriteLine("  Sound system: on");
        public void Off() => Console.WriteLine("  Sound system: off");
        public void SetSurroundSound() => Console.WriteLine("  Sound system: surround 5.1");
        public void SetVolume(int level) => Console.WriteLine($"  Sound system: volume {level}");
    }
}
