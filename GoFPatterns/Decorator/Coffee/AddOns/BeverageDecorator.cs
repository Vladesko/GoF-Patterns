namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Decorador base: implementa la misma interfaz que el componente y guarda el objeto envuelto.
    // Por defecto simplemente delega todas las llamadas en el objeto envuelto.
    public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
    {
        protected IBeverage Beverage { get; } = beverage ?? throw new ArgumentNullException(nameof(beverage));

        public virtual string GetDescription() => Beverage.GetDescription();

        public virtual decimal GetCost() => Beverage.GetCost();
    }
}
