namespace GoFPatterns.Singleton.Configuration.Services
{
    // Cliente: otro servicio que obtiene la misma instancia de la configuración.
    public class PriceService
    {
        public void ShowPrice(decimal amount)
        {
            var config = AppConfiguration.Instance;
            Console.WriteLine($"  [PriceService] config {config.InstanceId.ToString()[..8]}: precio {amount} {config.Get("Currency")}");
        }
    }
}
