namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Concrete strategy - courier, price depends on distance.
    public class CourierDeliveryStrategy : IDeliveryStrategy
    {
        public string Name => "Courier";

        public decimal CalculateCost(Parcel parcel) => 10m + parcel.DistanceKm * 0.05m + parcel.WeightKg * 0.5m;

        public int EstimateDays(Parcel parcel) => parcel.DistanceKm < 50 ? 1 : 2;
    }
}
