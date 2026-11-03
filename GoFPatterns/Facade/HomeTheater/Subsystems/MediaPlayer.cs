namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Subsystem class.
    public class MediaPlayer
    {
        public void On() => Console.WriteLine("  Media player: on");
        public void Off() => Console.WriteLine("  Media player: off");
        public void Play(string movie) => Console.WriteLine($"  Media player: playing \"{movie}\"");
        public void Stop() => Console.WriteLine("  Media player: stop");
        public void Eject() => Console.WriteLine("  Media player: eject disk");
    }
}
