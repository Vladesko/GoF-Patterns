namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsystem – Lager.
    public class InventoryService
    {
        private readonly Dictionary<string, int> _stock = new() { ["Laptop"] = 2, ["Smartphone"] = 0 };

        public bool IsAvailable(string product, int count) =>
            _stock.TryGetValue(product, out var available) && available >= count;

        public void Reserve(string product, int count)
        {
            _stock[product] -= count;
            Console.WriteLine($"  Lager: {count} x {product} reserviert, verbleibend {_stock[product]}");
        }

        public void Release(string product, int count)
        {
            _stock[product] += count;
            Console.WriteLine($"  Lager: Reservierung von {product} wurde storniert");
        }
    }
}
