using GoFPatterns.Singleton.Configuration.Services;

namespace GoFPatterns.Singleton.Configuration
{
    public class DemonstrateConfiguration
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Singleton: Configuration (Lazy<T>) ===\n");

            var cart = new CartService();
            var prices = new PriceService();

            // Configuration is loaded only on the first access.
            // (if demo is started again, configuration is already loaded and nothing is printed)
            Console.WriteLine("Services are created. Configuration will be loaded on the first access:");
            cart.AddItem(10);
            prices.ShowPrice(99.9m);

            // Change in one place is visible everywhere, because there is only one instance.
            AppConfiguration.Instance.Set("Currency", "USD");
            prices.ShowPrice(99.9m);

            // Many threads still get the same instance.
            var ids = new System.Collections.Concurrent.ConcurrentBag<Guid>();
            Parallel.For(0, 100, _ => ids.Add(AppConfiguration.Instance.InstanceId));
            Console.WriteLine($"\nUnique instances from 100 parallel calls: {ids.Distinct().Count()}");
        }
    }
}
