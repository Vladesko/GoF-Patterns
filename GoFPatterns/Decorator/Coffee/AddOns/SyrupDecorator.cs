namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Decorator concreto con un proprio stato - il gusto dello sciroppo.
    public class SyrupDecorator(IBeverage beverage, string flavor) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + sciroppo ({flavor})";

        public override decimal GetCost() => base.GetCost() + 0.70m;
    }
}
