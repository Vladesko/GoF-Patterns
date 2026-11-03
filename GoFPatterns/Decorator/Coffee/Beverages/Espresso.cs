namespace GoFPatterns.Decorator.Coffee.Beverages
{
    // Componente concreto: bebida básica que envolverán los decoradores.
    public class Espresso : IBeverage
    {
        public string GetDescription() => "Espresso";

        public decimal GetCost() => 2.00m;
    }
}
