namespace GoFPatterns.Strategy.Delivery
{
    // Context - uses strategy through interface. Strategy can be replaced at runtime.
    public class DeliveryCalculator(IDeliveryStrategy strategy)
    {
        private IDeliveryStrategy _strategy = strategy;

        public void SetStrategy(IDeliveryStrategy strategy)
        {
            ArgumentNullException.ThrowIfNull(strategy);
            _strategy = strategy;
        }

        public string GetQuote(Parcel parcel) =>
            $"{_strategy.Name,-13} cost: {_strategy.CalculateCost(parcel),6:F2}, days: {_strategy.EstimateDays(parcel)}";
    }
}
