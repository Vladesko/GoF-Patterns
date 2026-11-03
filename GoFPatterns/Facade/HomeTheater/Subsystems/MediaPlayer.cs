namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Classe del sottosistema.
    public class MediaPlayer
    {
        public void On() => Console.WriteLine("  Lettore multimediale: acceso");
        public void Off() => Console.WriteLine("  Lettore multimediale: spento");
        public void Play(string movie) => Console.WriteLine($"  Lettore multimediale: riproduzione di \"{movie}\"");
        public void Stop() => Console.WriteLine("  Lettore multimediale: stop");
        public void Eject() => Console.WriteLine("  Lettore multimediale: espulsione del disco");
    }
}
