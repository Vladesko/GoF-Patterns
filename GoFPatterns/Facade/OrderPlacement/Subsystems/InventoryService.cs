namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Sottosistema - magazzino.
    public class InventoryService
    {
        private readonly Dictionary<string, int> _stock = new() { ["Portatile"] = 2, ["Telefono"] = 0 };

        public bool IsAvailable(string product, int count) =>
            _stock.TryGetValue(product, out var available) && available >= count;

        public void Reserve(string product, int count)
        {
            _stock[product] -= count;
            Console.WriteLine($"  Magazzino: prenotato {count} x {product}, rimanenti {_stock[product]}");
        }

        public void Release(string product, int count)
        {
            _stock[product] += count;
            Console.WriteLine($"  Magazzino: la prenotazione di {product} è annullata");
        }
    }
}
