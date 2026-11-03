using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    // Fassade – einfache Schnittstelle zu einem komplexen Subsystem.
    // Sie weiß, welche Objekte des Subsystems in welcher Reihenfolge aufzurufen sind.
    public class HomeTheaterFacade(Projector projector, SoundSystem sound, MediaPlayer player, Lights lights)
    {
        public void WatchMovie(string movie)
        {
            Console.WriteLine($"Vorbereitung auf den Film \"{movie}\"...");
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
            Console.WriteLine("Heimkino wird heruntergefahren...");
            player.Stop();
            player.Eject();
            player.Off();
            sound.Off();
            projector.Off();
            lights.On();
        }
    }
}
