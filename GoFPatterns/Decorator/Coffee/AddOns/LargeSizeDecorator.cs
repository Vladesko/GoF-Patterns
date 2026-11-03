namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Concrete decorator which changes the result of wrapped object instead of adding to it.
    // Order of decorators matters: large size multiplies cost of everything inside.
    public class LargeSizeDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"Large ({base.GetDescription()})";

        public override decimal GetCost() => base.GetCost() * 1.5m;
    }
}
