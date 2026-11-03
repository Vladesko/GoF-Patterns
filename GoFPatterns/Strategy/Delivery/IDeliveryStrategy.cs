namespace GoFPatterns.Strategy.Delivery
{
    // Strategy - interfaccia comune per tutti gli algoritmi di calcolo del costo di consegna.
    public interface IDeliveryStrategy
    {
        string Name { get; }
        decimal CalculateCost(Parcel parcel);
        int EstimateDays(Parcel parcel);
    }
}
