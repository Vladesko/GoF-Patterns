namespace GoFPatterns.Strategy.Delivery
{
    // Данные для стратегий.
    public record Parcel(decimal WeightKg, int DistanceKm, decimal DeclaredValue);
}
