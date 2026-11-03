namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Concrete class which uses hooks: lower water temperature and no condiments.
    public class GreenTea : HotBeverage
    {
        protected override int WaterTemperature => 80;

        protected override void Brew() => Console.WriteLine("  steep green tea leaves for 2 minutes");

        protected override void AddCondiments() => Console.WriteLine("  add jasmine");

        protected override bool CustomerWantsCondiments() => false;
    }
}
