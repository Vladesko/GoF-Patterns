namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Concrete component - another basic beverage.
    public class BlackTea : IBeverage
    {
        public string GetDescription() => "Black tea";

        public decimal GetCost() => 1.50m;
    }
}
