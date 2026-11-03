namespace GoFPatterns.Strategy.Delivery
{
    // Context - usa la strategy tramite un'interfaccia. La strategy può essere sostituita a runtime.
    public class DeliveryCalculator(IDeliveryStrategy strategy)
    {
        private IDeliveryStrategy _strategy = strategy;

        public void SetStrategy(IDeliveryStrategy strategy)
        {
            ArgumentNullException.ThrowIfNull(strategy);
            _strategy = strategy;
        }

        public string GetQuote(Parcel parcel) =>
            $"{_strategy.Name,-13} costo: {_strategy.CalculateCost(parcel),6:F2}, giorni: {_strategy.EstimateDays(parcel)}";
    }
}
