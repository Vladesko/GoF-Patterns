namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Concrete decorator with its own state - flavor of syrup.
    public class SyrupDecorator(IBeverage beverage, string flavor) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + {flavor} syrup";

        public override decimal GetCost() => base.GetCost() + 0.70m;
    }
}
