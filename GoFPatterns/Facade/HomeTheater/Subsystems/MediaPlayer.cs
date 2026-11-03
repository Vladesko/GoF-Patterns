namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Clase del subsistema.
    public class MediaPlayer
    {
        public void On() => Console.WriteLine("  Reproductor: encendido");
        public void Off() => Console.WriteLine("  Reproductor: apagado");
        public void Play(string movie) => Console.WriteLine($"  Reproductor: reproduciendo \"{movie}\"");
        public void Stop() => Console.WriteLine("  Reproductor: detenido");
        public void Eject() => Console.WriteLine("  Reproductor: expulsando el disco");
    }
}
