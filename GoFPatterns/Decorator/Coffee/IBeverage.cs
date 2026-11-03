namespace GoFPatterns.Decorator.Coffee
{
    // Component - interfaccia comune per le bevande e per i loro decorator (extra).
    public interface IBeverage
    {
        string GetDescription();
        decimal GetCost();
    }
}
