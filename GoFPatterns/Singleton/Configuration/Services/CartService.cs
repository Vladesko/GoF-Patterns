namespace GoFPatterns.Singleton.Configuration.Services
{
    // Cliente: usa el punto de acceso global del singleton.
    public class CartService
    {
        public void AddItem(int itemsInCart)
        {
            var config = AppConfiguration.Instance;
            var max = int.Parse(config.Get("MaxItemsInCart"));
            var result = itemsInCart < max ? "añadido" : "rechazado, el carrito está lleno";
            Console.WriteLine($"  [CartService] config {config.InstanceId.ToString()[..8]}: artículo {result} (máx. {max})");
        }
    }
}
