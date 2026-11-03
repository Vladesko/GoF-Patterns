namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Concrete strategy - pickup point. Free for expensive orders.
    public class PickupPointStrategy : IDeliveryStrategy
    {
        private const decimal FreeDeliveryThreshold = 100m;

        public string Name => "Pickup point";

        public decimal CalculateCost(Parcel parcel) =>
            parcel.DeclaredValue >= FreeDeliveryThreshold ? 0m : 2.5m;

        public int EstimateDays(Parcel parcel) => 2;
    }
}
