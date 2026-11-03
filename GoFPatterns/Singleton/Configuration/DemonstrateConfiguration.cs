using GoFPatterns.Singleton.Configuration.Services;

namespace GoFPatterns.Singleton.Configuration
{
    public class DemonstrateConfiguration
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Singleton: configuración (Lazy<T>) ===\n");

            var cart = new CartService();
            var prices = new PriceService();

            // La configuración se carga solo en el primer acceso.
            // (si la demo se vuelve a ejecutar, la configuración ya está cargada y no se imprime nada)
            Console.WriteLine("Los servicios están creados. La configuración se cargará en el primer acceso:");
            cart.AddItem(10);
            prices.ShowPrice(99.9m);

            // Un cambio en un lugar es visible en todas partes, porque solo hay una instancia.
            AppConfiguration.Instance.Set("Currency", "USD");
            prices.ShowPrice(99.9m);

            // Muchos hilos siguen obteniendo la misma instancia.
            var ids = new System.Collections.Concurrent.ConcurrentBag<Guid>();
            Parallel.For(0, 100, _ => ids.Add(AppConfiguration.Instance.InstanceId));
            Console.WriteLine($"\nInstancias únicas en 100 llamadas paralelas: {ids.Distinct().Count()}");
        }
    }
}
