namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Decorador concreto que modifica el resultado del objeto envuelto en lugar de añadirle algo.
    // El orden de los decoradores importa: el tamaño grande multiplica el coste de todo lo que hay dentro.
    public class LargeSizeDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"Grande ({base.GetDescription()})";

        public override decimal GetCost() => base.GetCost() * 1.5m;
    }
}
