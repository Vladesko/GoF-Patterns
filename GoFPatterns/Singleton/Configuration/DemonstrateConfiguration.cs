using GoFPatterns.Singleton.Configuration.Services;

namespace GoFPatterns.Singleton.Configuration
{
    public class DemonstrateConfiguration
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Одиночка: конфигурация (Lazy<T>) ===\n");

            var cart = new CartService();
            var prices = new PriceService();

            // Конфигурация загружается только при первом обращении.
            // (если запустить демонстрацию снова, конфигурация уже загружена и ничего не выводится)
            Console.WriteLine("Сервисы созданы. Конфигурация будет загружена при первом обращении:");
            cart.AddItem(10);
            prices.ShowPrice(99.9m);

            // Изменение в одном месте видно везде, потому что экземпляр только один.
            AppConfiguration.Instance.Set("Currency", "USD");
            prices.ShowPrice(99.9m);

            // Множество потоков всё равно получают один и тот же экземпляр.
            var ids = new System.Collections.Concurrent.ConcurrentBag<Guid>();
            Parallel.For(0, 100, _ => ids.Add(AppConfiguration.Instance.InstanceId));
            Console.WriteLine($"\nУникальных экземпляров из 100 параллельных вызовов: {ids.Distinct().Count()}");
        }
    }
}
