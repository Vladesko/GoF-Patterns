namespace GoFPatterns.Strategy.Delivery
{
    // Datos para las estrategias.
    public record Parcel(decimal WeightKg, int DistanceKm, decimal DeclaredValue);
}
