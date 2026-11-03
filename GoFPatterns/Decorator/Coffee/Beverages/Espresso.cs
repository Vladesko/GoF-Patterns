namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Конкретный компонент - базовый напиток, который будет обёрнут декораторами.
    public class Espresso : IBeverage
    {
        public string GetDescription() => "Эспрессо";

        public decimal GetCost() => 2.00m;
    }
}
