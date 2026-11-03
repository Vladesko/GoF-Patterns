namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Конкретный декоратор, который изменяет результат обёрнутого объекта, а не дополняет его.
    // Порядок декораторов важен: большой размер умножает стоимость всего, что внутри.
    public class LargeSizeDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"Большой ({base.GetDescription()})";

        public override decimal GetCost() => base.GetCost() * 1.5m;
    }
}
