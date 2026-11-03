namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Класс подсистемы.
    public class MediaPlayer
    {
        public void On() => Console.WriteLine("  Медиаплеер: включён");
        public void Off() => Console.WriteLine("  Медиаплеер: выключен");
        public void Play(string movie) => Console.WriteLine($"  Медиаплеер: воспроизведение \"{movie}\"");
        public void Stop() => Console.WriteLine("  Медиаплеер: стоп");
        public void Eject() => Console.WriteLine("  Медиаплеер: извлечение диска");
    }
}
