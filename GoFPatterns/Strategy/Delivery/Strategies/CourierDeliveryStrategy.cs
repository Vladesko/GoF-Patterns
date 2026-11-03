namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Estrategia concreta: mensajero; el precio depende de la distancia.
    public class CourierDeliveryStrategy : IDeliveryStrategy
    {
        public string Name => "Mensajero";

        public decimal CalculateCost(Parcel parcel) => 10m + parcel.DistanceKm * 0.05m + parcel.WeightKg * 0.5m;

        public int EstimateDays(Parcel parcel) => parcel.DistanceKm < 50 ? 1 : 2;
    }
}
