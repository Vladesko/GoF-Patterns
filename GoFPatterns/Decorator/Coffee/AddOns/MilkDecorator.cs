namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Конкретный декоратор - добавляет поведение до или после вызова обёрнутого объекта.
    public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"{base.GetDescription()} + молоко";

        public override decimal GetCost() => base.GetCost() + 0.50m;
    }
}
