namespace GoFPatterns.Strategy.Delivery
{
    // Стратегия - общий интерфейс для всех алгоритмов расчёта стоимости доставки.
    public interface IDeliveryStrategy
    {
        string Name { get; }
        decimal CalculateCost(Parcel parcel);
        int EstimateDays(Parcel parcel);
    }
}
