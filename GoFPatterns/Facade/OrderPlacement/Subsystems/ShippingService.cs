namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsystem - delivery.
    public class ShippingService
    {
        public decimal CalculateCost(string address) => address.Contains("Moscow") ? 0m : 15m;

        public string CreateShipment(string product, string address)
        {
            var trackingNumber = $"TRK-{Random.Shared.Next(10000, 99999)}";
            Console.WriteLine($"  Shipping: {product} -> {address}, tracking {trackingNumber}");
            return trackingNumber;
        }
    }
}
