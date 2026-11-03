namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Decorador concreto: añade comportamiento antes o después de la llamada al objeto envuelto.
    public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + leche";

        public override decimal GetCost() => base.GetCost() + 0.50m;
    }
}
