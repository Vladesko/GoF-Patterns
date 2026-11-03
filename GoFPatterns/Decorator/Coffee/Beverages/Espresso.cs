namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Concrete component - basic beverage which will be wrapped by decorators.
    public class Espresso : IBeverage
    {
        public string GetDescription() => "Espresso";

        public decimal GetCost() => 2.00m;
    }
}
