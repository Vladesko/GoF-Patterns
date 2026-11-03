namespace GoFPatterns.Decorator.Coffee
{
    // Komponente – gemeinsame Schnittstelle für Getränke und ihre Dekorierer (Extras).
    public interface IBeverage
    {
        string GetDescription();
        decimal GetCost();
    }
}
