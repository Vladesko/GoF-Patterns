using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    // Facade - interfaccia semplice per un sottosistema complesso.
    // Sa quali oggetti del sottosistema chiamare e in quale ordine.
    public class HomeTheaterFacade(Projector projector, SoundSystem sound, MediaPlayer player, Lights lights)
    {
        public void WatchMovie(string movie)
        {
            Console.WriteLine($"Preparati a guardare \"{movie}\"...");
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
            Console.WriteLine("Spegnimento dell'home theater...");
            player.Stop();
            player.Eject();
            player.Off();
            sound.Off();
            projector.Off();
            lights.On();
        }
    }
}
