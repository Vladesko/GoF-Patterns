namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Component concreto - bevanda di base che verrà avvolta dai decorator.
    public class Espresso : IBeverage
    {
        public string GetDescription() => "Espresso";

        public decimal GetCost() => 2.00m;
    }
}
