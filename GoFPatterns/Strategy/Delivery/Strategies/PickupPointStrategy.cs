namespace GoFPatterns.Strategy.Delivery.Strategies
{
    // Estrategia concreta: punto de recogida. Gratis para pedidos caros.
    public class PickupPointStrategy : IDeliveryStrategy
    {
        private const decimal FreeDeliveryThreshold = 100m;

        public string Name => "Punto de recogida";

        public decimal CalculateCost(Parcel parcel) =>
            parcel.DeclaredValue >= FreeDeliveryThreshold ? 0m : 2.5m;

        public int EstimateDays(Parcel parcel) => 2;
    }
}
