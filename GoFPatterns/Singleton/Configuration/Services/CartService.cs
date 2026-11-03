namespace GoFPatterns.Singleton.Configuration.Services
{
    // Клиент - использует глобальную точку доступа одиночки.
    public class CartService
    {
        public void AddItem(int itemsInCart)
        {
            var config = AppConfiguration.Instance;
            var max = int.Parse(config.Get("MaxItemsInCart"));
            var result = itemsInCart < max ? "добавлен" : "отклонён, корзина заполнена";
            Console.WriteLine($"  [CartService] конфиг {config.InstanceId.ToString()[..8]}: товар {result} (максимум {max})");
        }
    }
}
