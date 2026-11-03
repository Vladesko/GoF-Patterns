namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Component concreto - un'altra bevanda di base.
    public class BlackTea : IBeverage
    {
        public string GetDescription() => "Tè nero";

        public decimal GetCost() => 1.50m;
    }
}
