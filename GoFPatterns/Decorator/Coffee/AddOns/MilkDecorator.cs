namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Concrete decorator - adds behavior before or after the call of wrapped object.
    public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + milk";

        public override decimal GetCost() => base.GetCost() + 0.50m;
    }
}
