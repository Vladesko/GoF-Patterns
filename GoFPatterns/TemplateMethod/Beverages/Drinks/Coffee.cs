namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Concrete class with hook that depends on customer's choice.
    public class Coffee(bool withMilk) : HotBeverage
    {
        protected override int WaterTemperature => 92;

        protected override void Brew() => Console.WriteLine("  drip coffee through filter");

        protected override void AddCondiments() => Console.WriteLine("  add sugar and milk");

        protected override bool CustomerWantsCondiments() => withMilk;
    }
}
