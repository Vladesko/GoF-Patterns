namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Конкретный декоратор с собственным состоянием - вкусом сиропа.
    public class SyrupDecorator(IBeverage beverage, string flavor) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + сироп ({flavor})";

        public override decimal GetCost() => base.GetCost() + 0.70m;
    }
}
