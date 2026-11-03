using GoFPatterns.Decorator.Coffee.AddOns;
using GoFPatterns.Decorator.Coffee.Beverages;

namespace GoFPatterns.Decorator.Coffee
{
    public class DemonstrateCoffee
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Decorator: Caffetteria ===\n");

            IBeverage espresso = new Espresso();
            Print(espresso);

            // I decorator si avvolgono l'un l'altro come una matrioska.
            IBeverage latte = new MilkDecorator(new MilkDecorator(new Espresso()));
            Print(latte);

            IBeverage vanillaLatte = new SyrupDecorator(latte, "vaniglia");
            Print(vanillaLatte);

            IBeverage largeVanillaLatte = new LargeSizeDecorator(vanillaLatte);
            Print(largeVanillaLatte);

            // Gli stessi extra possono essere usati con qualsiasi bevanda.
            Print(new SyrupDecorator(new MilkDecorator(new BlackTea()), "caramello"));

            // L'ordine dei decorator conta.
            Console.WriteLine("\nL'ordine conta:");
            Print(new LargeSizeDecorator(new SyrupDecorator(new Espresso(), "menta")));
            Print(new SyrupDecorator(new LargeSizeDecorator(new Espresso()), "menta"));
        }

        private static void Print(IBeverage beverage) =>
            Console.WriteLine($"{beverage.GetDescription(),-55} {beverage.GetCost():F2}");
    }
}
