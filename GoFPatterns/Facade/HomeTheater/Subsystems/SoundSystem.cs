namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Classe del sottosistema.
    public class SoundSystem
    {
        public void On() => Console.WriteLine("  Impianto audio: acceso");
        public void Off() => Console.WriteLine("  Impianto audio: spento");
        public void SetSurroundSound() => Console.WriteLine("  Impianto audio: surround 5.1");
        public void SetVolume(int level) => Console.WriteLine($"  Impianto audio: volume {level}");
    }
}
