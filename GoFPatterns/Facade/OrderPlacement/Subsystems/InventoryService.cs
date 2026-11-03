namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Подсистема - склад.
    public class InventoryService
    {
        private readonly Dictionary<string, int> _stock = new() { ["Ноутбук"] = 2, ["Телефон"] = 0 };

        public bool IsAvailable(string product, int count) =>
            _stock.TryGetValue(product, out var available) && available >= count;

        public void Reserve(string product, int count)
        {
            _stock[product] -= count;
            Console.WriteLine($"  Склад: зарезервировано {count} x {product}, осталось {_stock[product]}");
        }

        public void Release(string product, int count)
        {
            _stock[product] += count;
            Console.WriteLine($"  Склад: резерв товара {product} отменён");
        }
    }
}
