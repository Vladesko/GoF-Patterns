namespace GoFPatterns.Singleton.Configuration.Services
{
    // Client - another service, which gets the same instance of configuration.
    public class PriceService
    {
        public void ShowPrice(decimal amount)
        {
            var config = AppConfiguration.Instance;
            Console.WriteLine($"  [PriceService] config {config.InstanceId.ToString()[..8]}: price {amount} {config.Get("Currency")}");
        }
    }
}
