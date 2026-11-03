namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsistema: envío.
    public class ShippingService
    {
        public decimal CalculateCost(string address) => address.Contains("Moscow") ? 0m : 15m;

        public string CreateShipment(string product, string address)
        {
            var trackingNumber = $"TRK-{Random.Shared.Next(10000, 99999)}";
            Console.WriteLine($"  Envío: {product} -> {address}, seguimiento {trackingNumber}");
            return trackingNumber;
        }
    }
}
