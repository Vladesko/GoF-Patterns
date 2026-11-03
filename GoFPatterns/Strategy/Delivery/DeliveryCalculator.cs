namespace GoFPatterns.Strategy.Delivery
{
    // Kontext – nutzt die Strategie über die Schnittstelle. Die Strategie kann zur Laufzeit ausgetauscht werden.
    public class DeliveryCalculator(IDeliveryStrategy strategy)
    {
        private IDeliveryStrategy _strategy = strategy;

        public void SetStrategy(IDeliveryStrategy strategy)
        {
            ArgumentNullException.ThrowIfNull(strategy);
            _strategy = strategy;
        }

        public string GetQuote(Parcel parcel) =>
            $"{_strategy.Name,-13} Kosten: {_strategy.CalculateCost(parcel),6:F2}, Tage: {_strategy.EstimateDays(parcel)}";
    }
}
