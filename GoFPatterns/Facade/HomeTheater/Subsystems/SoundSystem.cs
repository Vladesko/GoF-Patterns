namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Clase del subsistema.
    public class SoundSystem
    {
        public void On() => Console.WriteLine("  Sistema de sonido: encendido");
        public void Off() => Console.WriteLine("  Sistema de sonido: apagado");
        public void SetSurroundSound() => Console.WriteLine("  Sistema de sonido: envolvente 5.1");
        public void SetVolume(int level) => Console.WriteLine($"  Sistema de sonido: volumen {level}");
    }
}
