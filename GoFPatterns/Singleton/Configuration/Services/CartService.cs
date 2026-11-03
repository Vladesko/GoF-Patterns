namespace GoFPatterns.Singleton.Configuration.Services
{
    // Client - uses global access point of the singleton.
    public class CartService
    {
        public void AddItem(int itemsInCart)
        {
            var config = AppConfiguration.Instance;
            var max = int.Parse(config.Get("MaxItemsInCart"));
            var result = itemsInCart < max ? "added" : "rejected, cart is full";
            Console.WriteLine($"  [CartService] config {config.InstanceId.ToString()[..8]}: item {result} (max {max})");
        }
    }
}
