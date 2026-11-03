namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Klasse des Subsystems.
    public class MediaPlayer
    {
        public void On() => Console.WriteLine("  Mediaplayer: an");
        public void Off() => Console.WriteLine("  Mediaplayer: aus");
        public void Play(string movie) => Console.WriteLine($"  Mediaplayer: spielt \"{movie}\" ab");
        public void Stop() => Console.WriteLine("  Mediaplayer: Stopp");
        public void Eject() => Console.WriteLine("  Mediaplayer: Disc wird ausgeworfen");
    }
}
