using GoFPatterns.Strategy.Delivery.Strategies;

namespace GoFPatterns.Strategy.Delivery
{
    public class DemonstrateDelivery
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Стратегия: стоимость доставки ===\n");

            var parcel = new Parcel(WeightKg: 4, DistanceKm: 30, DeclaredValue: 120);
            Console.WriteLine($"Посылка: {parcel}\n");

            IDeliveryStrategy[] strategies = [new PostDeliveryStrategy(), new CourierDeliveryStrategy(), new PickupPointStrategy()];

            // Один и тот же контекст с разными стратегиями.
            var calculator = new DeliveryCalculator(strategies[0]);
            foreach (var strategy in strategies)
            {
                calculator.SetStrategy(strategy);
                Console.WriteLine($"  {calculator.GetQuote(parcel)}");
            }

            // Стратегию можно выбирать автоматически по какому-либо правилу.
            var cheapest = strategies.MinBy(s => s.CalculateCost(parcel))!;
            var fastest = strategies.MinBy(s => s.EstimateDays(parcel))!;
            Console.WriteLine($"\nСамая дешёвая: {cheapest.Name}, самая быстрая: {fastest.Name}");
        }
    }
}
