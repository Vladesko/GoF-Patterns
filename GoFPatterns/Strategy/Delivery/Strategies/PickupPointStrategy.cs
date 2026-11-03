namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Конкретная стратегия - пункт выдачи. Бесплатно для дорогих заказов.
    public class PickupPointStrategy : IDeliveryStrategy
    {
        private const decimal FreeDeliveryThreshold = 100m;

        public string Name => "Пункт выдачи";

        public decimal CalculateCost(Parcel parcel) =>
            parcel.DeclaredValue >= FreeDeliveryThreshold ? 0m : 2.5m;

        public int EstimateDays(Parcel parcel) => 2;
    }
}
