namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Base decorator - implements the same interface as component and holds wrapped object.
    // By default it just delegates all calls to the wrapped object.
    public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
    {
        protected IBeverage Beverage { get; } = beverage ?? throw new ArgumentNullException(nameof(beverage));

        public virtual string GetDescription() => Beverage.GetDescription();

        public virtual decimal GetCost() => Beverage.GetCost();
    }
}
