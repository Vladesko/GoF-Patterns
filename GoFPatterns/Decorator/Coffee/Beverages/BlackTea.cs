namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Componente concreto: otra bebida básica.
    public class BlackTea : IBeverage
    {
        public string GetDescription() => "Té negro";

        public decimal GetCost() => 1.50m;
    }
}
