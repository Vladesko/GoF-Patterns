namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Базовый декоратор - реализует тот же интерфейс, что и компонент, и хранит обёрнутый объект.
    // По умолчанию он просто делегирует все вызовы обёрнутому объекту.
    public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
    {
        protected IBeverage Beverage { get; } = beverage ?? throw new ArgumentNullException(nameof(beverage));

        public virtual string GetDescription() => Beverage.GetDescription();

        public virtual decimal GetCost() => Beverage.GetCost();
    }
}
