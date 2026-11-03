using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    public class DemonstrateHomeTheater
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Facade: Heimkino ===\n");

            var theater = new HomeTheaterFacade(new Projector(), new SoundSystem(), new MediaPlayer(), new Lights());

            // Der Client ruft eine Methode statt zehn auf.
            theater.WatchMovie("Inception");
            Console.WriteLine();
            theater.EndMovie();

            // Die Subsysteme bleiben direkt zugänglich, falls der Client etwas Besonderes braucht.
            Console.WriteLine("\nDirekter Zugriff auf ein Subsystem ist weiterhin möglich:");
            new Lights().Dim(50);
        }
    }
}
