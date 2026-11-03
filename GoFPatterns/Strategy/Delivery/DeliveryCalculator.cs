namespace GoFPatterns.Strategy.Delivery
{
    // Contexto: usa la estrategia a través de una interfaz. La estrategia se puede sustituir en tiempo de ejecución.
    public class DeliveryCalculator(IDeliveryStrategy strategy)
    {
        private IDeliveryStrategy _strategy = strategy;

        public void SetStrategy(IDeliveryStrategy strategy)
        {
            ArgumentNullException.ThrowIfNull(strategy);
            _strategy = strategy;
        }

        public string GetQuote(Parcel parcel) =>
            $"{_strategy.Name,-17} coste: {_strategy.CalculateCost(parcel),6:F2}, días: {_strategy.EstimateDays(parcel)}";
    }
}
