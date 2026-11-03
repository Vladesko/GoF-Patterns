namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Конкретный класс.
    public class Tea : HotBeverage
    {
        protected override void Brew() => Console.WriteLine("  заварить чайный пакетик на 3 минуты");

        protected override void AddCondiments() => Console.WriteLine("  добавить лимон");
    }
}
