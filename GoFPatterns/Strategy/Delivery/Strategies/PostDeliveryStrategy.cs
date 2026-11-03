namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Strategy concreta - posta economica e lenta.
    public class PostDeliveryStrategy : IDeliveryStrategy
    {
        public string Name => "Posta";

        public decimal CalculateCost(Parcel parcel) => 3m + parcel.WeightKg * 1.2m;

        public int EstimateDays(Parcel parcel) => 3 + parcel.DistanceKm / 300;
    }
}
