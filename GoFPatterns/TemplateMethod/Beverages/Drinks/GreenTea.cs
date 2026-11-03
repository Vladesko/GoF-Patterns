namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Конкретный класс, который использует хуки: более низкая температура воды и без добавок.
    public class GreenTea : HotBeverage
    {
        protected override int WaterTemperature => 80;

        protected override void Brew() => Console.WriteLine("  заварить листья зелёного чая на 2 минуты");

        protected override void AddCondiments() => Console.WriteLine("  добавить жасмин");

        protected override bool CustomerWantsCondiments() => false;
    }
}
