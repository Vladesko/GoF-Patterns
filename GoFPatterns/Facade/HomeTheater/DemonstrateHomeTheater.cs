using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    public class DemonstrateHomeTheater
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Facade: Home theater ===\n");

            var theater = new HomeTheaterFacade(new Projector(), new SoundSystem(), new MediaPlayer(), new Lights());

            // Il client chiama un solo metodo invece di dieci.
            theater.WatchMovie("Inception");
            Console.WriteLine();
            theater.EndMovie();

            // I sottosistemi restano accessibili direttamente, se il client ha bisogno di qualcosa di particolare.
            Console.WriteLine("\nL'accesso diretto al sottosistema è ancora possibile:");
            new Lights().Dim(50);
        }
    }
}
