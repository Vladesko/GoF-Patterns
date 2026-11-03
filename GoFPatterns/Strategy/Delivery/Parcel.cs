namespace GoFPatterns.Strategy.Delivery
{
    // Daten für die Strategien.
    public record Parcel(decimal WeightKg, int DistanceKm, decimal DeclaredValue);
}
