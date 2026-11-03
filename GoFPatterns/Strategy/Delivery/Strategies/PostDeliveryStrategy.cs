namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Estrategia concreta: correo postal, barato y lento.
    public class PostDeliveryStrategy : IDeliveryStrategy
    {
        public string Name => "Correo postal";

        public decimal CalculateCost(Parcel parcel) => 3m + parcel.WeightKg * 1.2m;

        public int EstimateDays(Parcel parcel) => 3 + parcel.DistanceKm / 300;
    }
}
