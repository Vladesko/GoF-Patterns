namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Подсистема - доставка.
    public class ShippingService
    {
        public decimal CalculateCost(string address) => address.Contains("Москва") ? 0m : 15m;

        public string CreateShipment(string product, string address)
        {
            var trackingNumber = $"TRK-{Random.Shared.Next(10000, 99999)}";
            Console.WriteLine($"  Доставка: {product} -> {address}, трек-номер {trackingNumber}");
            return trackingNumber;
        }
    }
}
