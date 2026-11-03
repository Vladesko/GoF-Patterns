using GoFPatterns.Singleton.Configuration.Services;

namespace GoFPatterns.Singleton.Configuration
{
    public class DemonstrateConfiguration
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Singleton: Konfiguration (Lazy<T>) ===\n");

            var cart = new CartService();
            var prices = new PriceService();

            // Die Konfiguration wird erst beim ersten Zugriff geladen.
            // (wird die Demo erneut gestartet, ist die Konfiguration bereits geladen und es wird nichts ausgegeben)
            Console.WriteLine("Die Dienste sind erstellt. Die Konfiguration wird beim ersten Zugriff geladen:");
            cart.AddItem(10);
            prices.ShowPrice(99.9m);

            // Eine Änderung an einer Stelle ist überall sichtbar, weil es nur eine Instanz gibt.
            AppConfiguration.Instance.Set("Currency", "USD");
            prices.ShowPrice(99.9m);

            // Auch viele Threads erhalten dieselbe Instanz.
            var ids = new System.Collections.Concurrent.ConcurrentBag<Guid>();
            Parallel.For(0, 100, _ => ids.Add(AppConfiguration.Instance.InstanceId));
            Console.WriteLine($"\nEindeutige Instanzen bei 100 parallelen Aufrufen: {ids.Distinct().Count()}");
        }
    }
}
