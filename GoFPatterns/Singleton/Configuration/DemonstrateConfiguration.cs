using GoFPatterns.Singleton.Configuration.Services;

namespace GoFPatterns.Singleton.Configuration
{
    public class DemonstrateConfiguration
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Singleton: Configurazione (Lazy<T>) ===\n");

            var cart = new CartService();
            var prices = new PriceService();

            // La configurazione viene caricata solo al primo accesso.
            // (se la demo viene riavviata, la configurazione è già caricata e non viene stampato nulla)
            Console.WriteLine("I servizi sono stati creati. La configurazione verrà caricata al primo accesso:");
            cart.AddItem(10);
            prices.ShowPrice(99.9m);

            // Una modifica in un punto è visibile ovunque, perché esiste una sola istanza.
            AppConfiguration.Instance.Set("Currency", "USD");
            prices.ShowPrice(99.9m);

            // Anche molti thread ottengono la stessa istanza.
            var ids = new System.Collections.Concurrent.ConcurrentBag<Guid>();
            Parallel.For(0, 100, _ => ids.Add(AppConfiguration.Instance.InstanceId));
            Console.WriteLine($"\nIstanze uniche da 100 chiamate parallele: {ids.Distinct().Count()}");
        }
    }
}
