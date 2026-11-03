namespace GoFPatterns.Singleton.Configuration.Services
{
    // Client - usa il punto di accesso globale del singleton.
    public class CartService
    {
        public void AddItem(int itemsInCart)
        {
            var config = AppConfiguration.Instance;
            var max = int.Parse(config.Get("MaxItemsInCart"));
            var result = itemsInCart < max ? "aggiunto" : "rifiutato, il carrello è pieno";
            Console.WriteLine($"  [CartService] config {config.InstanceId.ToString()[..8]}: articolo {result} (max {max})");
        }
    }
}
