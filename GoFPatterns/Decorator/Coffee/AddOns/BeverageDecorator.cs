namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Decorator base - implementa la stessa interfaccia del component e contiene l'oggetto avvolto.
    // Di default delega semplicemente tutte le chiamate all'oggetto avvolto.
    public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
    {
        protected IBeverage Beverage { get; } = beverage ?? throw new ArgumentNullException(nameof(beverage));

        public virtual string GetDescription() => Beverage.GetDescription();

        public virtual decimal GetCost() => Beverage.GetCost();
    }
}
