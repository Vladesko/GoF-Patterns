namespace GoFPatterns.Strategy.Delivery
{
    // Контекст - использует стратегию через интерфейс. Стратегию можно заменить во время выполнения.
    public class DeliveryCalculator(IDeliveryStrategy strategy)
    {
        private IDeliveryStrategy _strategy = strategy;

        public void SetStrategy(IDeliveryStrategy strategy)
        {
            ArgumentNullException.ThrowIfNull(strategy);
            _strategy = strategy;
        }

        public string GetQuote(Parcel parcel) =>
            $"{_strategy.Name,-13} стоимость: {_strategy.CalculateCost(parcel),6:F2}, дней: {_strategy.EstimateDays(parcel)}";
    }
}
