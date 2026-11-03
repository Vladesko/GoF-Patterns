using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    public class DemonstrateHomeTheater
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Facade: Home theater ===\n");

            var theater = new HomeTheaterFacade(new Projector(), new SoundSystem(), new MediaPlayer(), new Lights());

            // Client calls one method instead of ten.
            theater.WatchMovie("Inception");
            Console.WriteLine();
            theater.EndMovie();

            // Subsystems are still available directly, if client needs something special.
            Console.WriteLine("\nDirect access to subsystem is still possible:");
            new Lights().Dim(50);
        }
    }
}
