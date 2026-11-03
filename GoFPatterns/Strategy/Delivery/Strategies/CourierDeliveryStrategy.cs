namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Konkrete Strategie – Kurier, der Preis hängt von der Entfernung ab.
    public class CourierDeliveryStrategy : IDeliveryStrategy
    {
        public string Name => "Kurier";

        public decimal CalculateCost(Parcel parcel) => 10m + parcel.DistanceKm * 0.05m + parcel.WeightKg * 0.5m;

        public int EstimateDays(Parcel parcel) => parcel.DistanceKm < 50 ? 1 : 2;
    }
}
