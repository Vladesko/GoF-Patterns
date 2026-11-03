namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Concrete class.
    public class Tea : HotBeverage
    {
        protected override void Brew() => Console.WriteLine("  steep the tea bag for 3 minutes");

        protected override void AddCondiments() => Console.WriteLine("  add lemon");
    }
}
