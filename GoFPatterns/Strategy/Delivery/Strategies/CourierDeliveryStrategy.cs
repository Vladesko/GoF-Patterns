namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Strategy concreta - corriere, il prezzo dipende dalla distanza.
    public class CourierDeliveryStrategy : IDeliveryStrategy
    {
        public string Name => "Corriere";

        public decimal CalculateCost(Parcel parcel) => 10m + parcel.DistanceKm * 0.05m + parcel.WeightKg * 0.5m;

        public int EstimateDays(Parcel parcel) => parcel.DistanceKm < 50 ? 1 : 2;
    }
}
