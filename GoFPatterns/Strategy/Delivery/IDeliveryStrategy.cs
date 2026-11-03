namespace GoFPatterns.Strategy.Delivery
{
    // Estrategia: interfaz común para todos los algoritmos de cálculo del coste de envío.
    public interface IDeliveryStrategy
    {
        string Name { get; }
        decimal CalculateCost(Parcel parcel);
        int EstimateDays(Parcel parcel);
    }
}
