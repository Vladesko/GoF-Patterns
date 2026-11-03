namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Konkrete Komponente – Grundgetränk, das von Dekorierern umhüllt wird.
    public class Espresso : IBeverage
    {
        public string GetDescription() => "Espresso";

        public decimal GetCost() => 2.00m;
    }
}
