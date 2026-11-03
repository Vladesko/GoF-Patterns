using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    // Фасад - простой интерфейс к сложной подсистеме.
    // Он знает, какие объекты подсистемы вызывать и в каком порядке.
    public class HomeTheaterFacade(Projector projector, SoundSystem sound, MediaPlayer player, Lights lights)
    {
        public void WatchMovie(string movie)
        {
            Console.WriteLine($"Готовимся к просмотру \"{movie}\"...");
            lights.Dim(10);
            projector.On();
            projector.SetInput("HDMI-1");
            projector.WideScreenMode();
            sound.On();
            sound.SetSurroundSound();
            sound.SetVolume(25);
            player.On();
            player.Play(movie);
        }

        public void EndMovie()
        {
            Console.WriteLine("Выключаем кинотеатр...");
            player.Stop();
            player.Eject();
            player.Off();
            sound.Off();
            projector.Off();
            lights.On();
        }
    }
}
