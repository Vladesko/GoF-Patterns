namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Konkrete Strategie – Abholstation. Kostenlos für teure Bestellungen.
    public class PickupPointStrategy : IDeliveryStrategy
    {
        private const decimal FreeDeliveryThreshold = 100m;

        public string Name => "Abholstation";

        public decimal CalculateCost(Parcel parcel) =>
            parcel.DeclaredValue >= FreeDeliveryThreshold ? 0m : 2.5m;

        public int EstimateDays(Parcel parcel) => 2;
    }
}
