namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Konkrete Strategie – günstige und langsame Post.
    public class PostDeliveryStrategy : IDeliveryStrategy
    {
        public string Name => "Post";

        public decimal CalculateCost(Parcel parcel) => 3m + parcel.WeightKg * 1.2m;

        public int EstimateDays(Parcel parcel) => 3 + parcel.DistanceKm / 300;
    }
}
