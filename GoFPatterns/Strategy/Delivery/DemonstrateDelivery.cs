using GoFPatterns.Strategy.Delivery.Strategies;

namespace GoFPatterns.Strategy.Delivery
{
    public class DemonstrateDelivery
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Strategy: Lieferkosten ===\n");

            var parcel = new Parcel(WeightKg: 4, DistanceKm: 30, DeclaredValue: 120);
            Console.WriteLine($"Paket: {parcel}\n");

            IDeliveryStrategy[] strategies = [new PostDeliveryStrategy(), new CourierDeliveryStrategy(), new PickupPointStrategy()];

            // Derselbe Kontext mit verschiedenen Strategien.
            var calculator = new DeliveryCalculator(strategies[0]);
            foreach (var strategy in strategies)
            {
                calculator.SetStrategy(strategy);
                Console.WriteLine($"  {calculator.GetQuote(parcel)}");
            }

            // Die Strategie kann auch automatisch nach einer Regel gewählt werden.
            var cheapest = strategies.MinBy(s => s.CalculateCost(parcel))!;
            var fastest = strategies.MinBy(s => s.EstimateDays(parcel))!;
            Console.WriteLine($"\nAm günstigsten: {cheapest.Name}, am schnellsten: {fastest.Name}");
        }
    }
}
