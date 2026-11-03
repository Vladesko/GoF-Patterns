using GoFPatterns.Strategy.Delivery.Strategies;

namespace GoFPatterns.Strategy.Delivery
{
    public class DemonstrateDelivery
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Strategy: coste de envío ===\n");

            var parcel = new Parcel(WeightKg: 4, DistanceKm: 30, DeclaredValue: 120);
            Console.WriteLine($"Paquete: {parcel}\n");

            IDeliveryStrategy[] strategies = [new PostDeliveryStrategy(), new CourierDeliveryStrategy(), new PickupPointStrategy()];

            // El mismo contexto con distintas estrategias.
            var calculator = new DeliveryCalculator(strategies[0]);
            foreach (var strategy in strategies)
            {
                calculator.SetStrategy(strategy);
                Console.WriteLine($"  {calculator.GetQuote(parcel)}");
            }

            // La estrategia se puede elegir automáticamente según alguna regla.
            var cheapest = strategies.MinBy(s => s.CalculateCost(parcel))!;
            var fastest = strategies.MinBy(s => s.EstimateDays(parcel))!;
            Console.WriteLine($"\nMás barata: {cheapest.Name}, más rápida: {fastest.Name}");
        }
    }
}
