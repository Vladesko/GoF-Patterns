using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    // Facade - simple interface for a complex subsystem.
    // It knows which subsystem objects to call and in which order.
    public class HomeTheaterFacade(Projector projector, SoundSystem sound, MediaPlayer player, Lights lights)
    {
        public void WatchMovie(string movie)
        {
            Console.WriteLine($"Get ready to watch \"{movie}\"...");
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
            Console.WriteLine("Shutting movie theater down...");
            player.Stop();
            player.Eject();
            player.Off();
            sound.Off();
            projector.Off();
            lights.On();
        }
    }
}
