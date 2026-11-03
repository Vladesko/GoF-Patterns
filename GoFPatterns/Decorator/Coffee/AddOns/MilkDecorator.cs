namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Konkreter Dekorierer – fügt Verhalten vor oder nach dem Aufruf des umhüllten Objekts hinzu.
    public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + Milch";

        public override decimal GetCost() => base.GetCost() + 0.50m;
    }
}
