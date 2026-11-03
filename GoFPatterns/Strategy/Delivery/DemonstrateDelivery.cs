using GoFPatterns.Strategy.Delivery.Strategies;

namespace GoFPatterns.Strategy.Delivery
{
    public class DemonstrateDelivery
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Strategy: Costo di consegna ===\n");

            var parcel = new Parcel(WeightKg: 4, DistanceKm: 30, DeclaredValue: 120);
            Console.WriteLine($"Pacco: {parcel}\n");

            IDeliveryStrategy[] strategies = [new PostDeliveryStrategy(), new CourierDeliveryStrategy(), new PickupPointStrategy()];

            // Lo stesso context con strategy diverse.
            var calculator = new DeliveryCalculator(strategies[0]);
            foreach (var strategy in strategies)
            {
                calculator.SetStrategy(strategy);
                Console.WriteLine($"  {calculator.GetQuote(parcel)}");
            }

            // La strategy può essere scelta automaticamente secondo una regola.
            var cheapest = strategies.MinBy(s => s.CalculateCost(parcel))!;
            var fastest = strategies.MinBy(s => s.EstimateDays(parcel))!;
            Console.WriteLine($"\nLa più economica: {cheapest.Name}, la più veloce: {fastest.Name}");
        }
    }
}
