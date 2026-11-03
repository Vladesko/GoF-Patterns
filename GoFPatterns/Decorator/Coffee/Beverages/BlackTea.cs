namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Konkrete Komponente – ein weiteres Grundgetränk.
    public class BlackTea : IBeverage
    {
        public string GetDescription() => "Schwarzer Tee";

        public decimal GetCost() => 1.50m;
    }
}
