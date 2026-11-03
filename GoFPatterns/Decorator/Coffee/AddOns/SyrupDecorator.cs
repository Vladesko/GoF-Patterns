namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Konkreter Dekorierer mit eigenem Zustand – der Geschmack des Sirups.
    public class SyrupDecorator(IBeverage beverage, string flavor) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + {flavor}sirup";

        public override decimal GetCost() => base.GetCost() + 0.70m;
    }
}
