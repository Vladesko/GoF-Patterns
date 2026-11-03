namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Basis-Dekorierer – implementiert dieselbe Schnittstelle wie die Komponente und hält das umhüllte Objekt.
    // Standardmäßig delegiert er einfach alle Aufrufe an das umhüllte Objekt.
    public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
    {
        protected IBeverage Beverage { get; } = beverage ?? throw new ArgumentNullException(nameof(beverage));

        public virtual string GetDescription() => Beverage.GetDescription();

        public virtual decimal GetCost() => Beverage.GetCost();
    }
}
