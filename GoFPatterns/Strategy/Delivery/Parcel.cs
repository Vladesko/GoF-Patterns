namespace GoFPatterns.Strategy.Delivery
{
    // Dati per le strategy.
    public record Parcel(decimal WeightKg, int DistanceKm, decimal DeclaredValue);
}
