namespace GoFPatterns.Strategy.Delivery
{
    // Strategy - common interface for all algorithms of delivery cost calculation.
    public interface IDeliveryStrategy
    {
        string Name { get; }
        decimal CalculateCost(Parcel parcel);
        int EstimateDays(Parcel parcel);
    }
}
