namespace GoFPatterns.Singleton.Configuration.Services
{
    // Client – nutzt den globalen Zugriffspunkt des Singletons.
    public class CartService
    {
        public void AddItem(int itemsInCart)
        {
            var config = AppConfiguration.Instance;
            var max = int.Parse(config.Get("MaxItemsInCart"));
            var result = itemsInCart < max ? "hinzugefügt" : "abgelehnt, Warenkorb ist voll";
            Console.WriteLine($"  [CartService] Konfiguration {config.InstanceId.ToString()[..8]}: Artikel {result} (max. {max})");
        }
    }
}
