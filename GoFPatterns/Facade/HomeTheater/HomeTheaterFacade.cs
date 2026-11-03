using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    // Fachada: interfaz sencilla para un subsistema complejo.
    // Sabe a qué objetos del subsistema llamar y en qué orden.
    public class HomeTheaterFacade(Projector projector, SoundSystem sound, MediaPlayer player, Lights lights)
    {
        public void WatchMovie(string movie)
        {
            Console.WriteLine($"Preparándose para ver \"{movie}\"...");
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
            Console.WriteLine("Apagando el cine en casa...");
            player.Stop();
            player.Eject();
            player.Off();
            sound.Off();
            projector.Off();
            lights.On();
        }
    }
}
