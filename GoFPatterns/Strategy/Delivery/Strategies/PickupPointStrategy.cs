namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Strategy concreta - punto di ritiro. Gratuito per gli ordini costosi.
    public class PickupPointStrategy : IDeliveryStrategy
    {
        private const decimal FreeDeliveryThreshold = 100m;

        public string Name => "Punto ritiro";

        public decimal CalculateCost(Parcel parcel) =>
            parcel.DeclaredValue >= FreeDeliveryThreshold ? 0m : 2.5m;

        public int EstimateDays(Parcel parcel) => 2;
    }
}
