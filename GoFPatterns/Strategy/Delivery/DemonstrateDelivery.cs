using GoFPatterns.Strategy.Delivery.Strategies;

namespace GoFPatterns.Strategy.Delivery
{
    public class DemonstrateDelivery
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Strategy: Delivery cost ===\n");

            var parcel = new Parcel(WeightKg: 4, DistanceKm: 30, DeclaredValue: 120);
            Console.WriteLine($"Parcel: {parcel}\n");

            IDeliveryStrategy[] strategies = [new PostDeliveryStrategy(), new CourierDeliveryStrategy(), new PickupPointStrategy()];

            // The same context with different strategies.
            var calculator = new DeliveryCalculator(strategies[0]);
            foreach (var strategy in strategies)
            {
                calculator.SetStrategy(strategy);
                Console.WriteLine($"  {calculator.GetQuote(parcel)}");
            }

            // Strategy can be chosen automatically by some rule.
            var cheapest = strategies.MinBy(s => s.CalculateCost(parcel))!;
            var fastest = strategies.MinBy(s => s.EstimateDays(parcel))!;
            Console.WriteLine($"\nCheapest: {cheapest.Name}, fastest: {fastest.Name}");
        }
    }
}
