namespace GoFPatterns.Decorator.Coffee
{
    // Component - common interface for beverages and for their decorators (add-ons).
    public interface IBeverage
    {
        string GetDescription();
        decimal GetCost();
    }
}
