namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Конкретный класс с хуком, который зависит от выбора покупателя.
    public class Coffee(bool withMilk) : HotBeverage
    {
        protected override int WaterTemperature => 92;

        protected override void Brew() => Console.WriteLine("  пропустить кофе через фильтр");

        protected override void AddCondiments() => Console.WriteLine("  добавить сахар и молоко");

        protected override bool CustomerWantsCondiments() => withMilk;
    }
}
