namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Decorator concreto - aggiunge comportamento prima o dopo la chiamata dell'oggetto avvolto.
    public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + latte";

        public override decimal GetCost() => base.GetCost() + 0.50m;
    }
}
