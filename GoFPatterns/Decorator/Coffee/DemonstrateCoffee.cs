using GoFPatterns.Decorator.Coffee.AddOns;
using GoFPatterns.Decorator.Coffee.Beverages;

namespace GoFPatterns.Decorator.Coffee
{
    public class DemonstrateCoffee
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Decorator: Coffee shop ===\n");

            IBeverage espresso = new Espresso();
            Print(espresso);

            // Decorators wrap each other like a matryoshka.
            IBeverage latte = new MilkDecorator(new MilkDecorator(new Espresso()));
            Print(latte);

            IBeverage vanillaLatte = new SyrupDecorator(latte, "vanilla");
            Print(vanillaLatte);

            IBeverage largeVanillaLatte = new LargeSizeDecorator(vanillaLatte);
            Print(largeVanillaLatte);

            // The same add-ons can be used with any beverage.
            Print(new SyrupDecorator(new MilkDecorator(new BlackTea()), "caramel"));

            // Order of decorators matters.
            Console.WriteLine("\nOrder matters:");
            Print(new LargeSizeDecorator(new SyrupDecorator(new Espresso(), "mint")));
            Print(new SyrupDecorator(new LargeSizeDecorator(new Espresso()), "mint"));
        }

        private static void Print(IBeverage beverage) =>
            Console.WriteLine($"{beverage.GetDescription(),-55} {beverage.GetCost():F2}");
    }
}
