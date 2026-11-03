using GoFPatterns.Facade.HomeTheater.Subsystems;

namespace GoFPatterns.Facade.HomeTheater
{
    public class DemonstrateHomeTheater
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Фасад: домашний кинотеатр ===\n");

            var theater = new HomeTheaterFacade(new Projector(), new SoundSystem(), new MediaPlayer(), new Lights());

            // Клиент вызывает один метод вместо десяти.
            theater.WatchMovie("Начало");
            Console.WriteLine();
            theater.EndMovie();

            // Подсистемы по-прежнему доступны напрямую, если клиенту нужно что-то особенное.
            Console.WriteLine("\nПрямой доступ к подсистеме по-прежнему возможен:");
            new Lights().Dim(50);
        }
    }
}
