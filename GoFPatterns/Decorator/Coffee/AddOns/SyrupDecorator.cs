namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Decorador concreto con estado propio: el sabor del sirope.
    public class SyrupDecorator(IBeverage beverage, string flavor) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + sirope de {flavor}";

        public override decimal GetCost() => base.GetCost() + 0.70m;
    }
}
