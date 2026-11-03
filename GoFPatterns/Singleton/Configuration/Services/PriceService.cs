namespace GoFPatterns.Singleton.Configuration.Services
{
    // Client – ein weiterer Dienst, der dieselbe Konfigurationsinstanz erhält.
    public class PriceService
    {
        public void ShowPrice(decimal amount)
        {
            var config = AppConfiguration.Instance;
            Console.WriteLine($"  [PriceService] Konfiguration {config.InstanceId.ToString()[..8]}: Preis {amount} {config.Get("Currency")}");
        }
    }
}
