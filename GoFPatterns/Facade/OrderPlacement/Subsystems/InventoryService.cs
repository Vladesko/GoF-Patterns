namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsystem - warehouse.
    public class InventoryService
    {
        private readonly Dictionary<string, int> _stock = new() { ["Laptop"] = 2, ["Phone"] = 0 };

        public bool IsAvailable(string product, int count) =>
            _stock.TryGetValue(product, out var available) && available >= count;

        public void Reserve(string product, int count)
        {
            _stock[product] -= count;
            Console.WriteLine($"  Inventory: reserved {count} x {product}, left {_stock[product]}");
        }

        public void Release(string product, int count)
        {
            _stock[product] += count;
            Console.WriteLine($"  Inventory: reservation of {product} is cancelled");
        }
    }
}
