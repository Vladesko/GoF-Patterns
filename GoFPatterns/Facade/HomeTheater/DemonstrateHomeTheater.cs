using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    public class DemonstrateHomeTheater
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Facade: cine en casa ===\n");

            var theater = new HomeTheaterFacade(new Projector(), new SoundSystem(), new MediaPlayer(), new Lights());

            // El cliente llama a un solo método en lugar de a diez.
            theater.WatchMovie("Origen");
            Console.WriteLine();
            theater.EndMovie();

            // Los subsistemas siguen disponibles directamente si el cliente necesita algo especial.
            Console.WriteLine("\nEl acceso directo al subsistema sigue siendo posible:");
            new Lights().Dim(50);
        }
    }
}
