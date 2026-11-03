namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Конкретная стратегия - дешёвая и медленная почта.
    public class PostDeliveryStrategy : IDeliveryStrategy
    {
        public string Name => "Почта";

        public decimal CalculateCost(Parcel parcel) => 3m + parcel.WeightKg * 1.2m;

        public int EstimateDays(Parcel parcel) => 3 + parcel.DistanceKm / 300;
    }
}
