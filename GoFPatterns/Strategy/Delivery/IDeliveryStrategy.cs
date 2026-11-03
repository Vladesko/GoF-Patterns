namespace GoFPatterns.Strategy.Delivery
{
    // Strategie – gemeinsame Schnittstelle für alle Algorithmen zur Berechnung der Lieferkosten.
    public interface IDeliveryStrategy
    {
        string Name { get; }
        decimal CalculateCost(Parcel parcel);
        int EstimateDays(Parcel parcel);
    }
}
