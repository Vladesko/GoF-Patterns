namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsistema: almacén.
    public class InventoryService
    {
        private readonly Dictionary<string, int> _stock = new() { ["Portátil"] = 2, ["Teléfono"] = 0 };

        public bool IsAvailable(string product, int count) =>
            _stock.TryGetValue(product, out var available) && available >= count;

        public void Reserve(string product, int count)
        {
            _stock[product] -= count;
            Console.WriteLine($"  Inventario: reservado {count} x {product}, quedan {_stock[product]}");
        }

        public void Release(string product, int count)
        {
            _stock[product] += count;
            Console.WriteLine($"  Inventario: se cancela la reserva de {product}");
        }
    }
}
