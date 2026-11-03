namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Конкретный компонент - ещё один базовый напиток.
    public class BlackTea : IBeverage
    {
        public string GetDescription() => "Чёрный чай";

        public decimal GetCost() => 1.50m;
    }
}
