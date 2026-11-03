using GoFPatterns.Decorator.Coffee.AddOns;
using GoFPatterns.Decorator.Coffee.Beverages;

namespace GoFPatterns.Decorator.Coffee
{
    public class DemonstrateCoffee
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Decorator: cafetería ===\n");

            IBeverage espresso = new Espresso();
            Print(espresso);

            // Los decoradores se envuelven unos a otros como una matrioska.
            IBeverage latte = new MilkDecorator(new MilkDecorator(new Espresso()));
            Print(latte);

            IBeverage vanillaLatte = new SyrupDecorator(latte, "vainilla");
            Print(vanillaLatte);

            IBeverage largeVanillaLatte = new LargeSizeDecorator(vanillaLatte);
            Print(largeVanillaLatte);

            // Los mismos complementos se pueden usar con cualquier bebida.
            Print(new SyrupDecorator(new MilkDecorator(new BlackTea()), "caramelo"));

            // El orden de los decoradores importa.
            Console.WriteLine("\nEl orden importa:");
            Print(new LargeSizeDecorator(new SyrupDecorator(new Espresso(), "menta")));
            Print(new SyrupDecorator(new LargeSizeDecorator(new Espresso()), "menta"));
        }

        private static void Print(IBeverage beverage) =>
            Console.WriteLine($"{beverage.GetDescription(),-55} {beverage.GetCost():F2}");
    }
}
