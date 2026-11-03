namespace GoFPatterns.Singleton.Configuration.Services
{
    // Клиент - другой сервис, который получает тот же экземпляр конфигурации.
    public class PriceService
    {
        public void ShowPrice(decimal amount)
        {
            var config = AppConfiguration.Instance;
            Console.WriteLine($"  [PriceService] конфиг {config.InstanceId.ToString()[..8]}: цена {amount} {config.Get("Currency")}");
        }
    }
}
