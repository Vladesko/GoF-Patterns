namespace GoFPatterns.Singleton.Configuration.Services
{
    // Client - un altro servizio, che ottiene la stessa istanza della configurazione.
    public class PriceService
    {
        public void ShowPrice(decimal amount)
        {
            var config = AppConfiguration.Instance;
            Console.WriteLine($"  [PriceService] config {config.InstanceId.ToString()[..8]}: prezzo {amount} {config.Get("Currency")}");
        }
    }
}
