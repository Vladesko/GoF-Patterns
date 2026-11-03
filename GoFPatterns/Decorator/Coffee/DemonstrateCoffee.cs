using GoFPatterns.Decorator.Coffee.AddOns;
using GoFPatterns.Decorator.Coffee.Beverages;

namespace GoFPatterns.Decorator.Coffee
{
    public class DemonstrateCoffee
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Decorator: Café ===\n");

            IBeverage espresso = new Espresso();
            Print(espresso);

            // Dekorierer umhüllen einander wie eine Matroschka.
            IBeverage latte = new MilkDecorator(new MilkDecorator(new Espresso()));
            Print(latte);

            IBeverage vanillaLatte = new SyrupDecorator(latte, "Vanille");
            Print(vanillaLatte);

            IBeverage largeVanillaLatte = new LargeSizeDecorator(vanillaLatte);
            Print(largeVanillaLatte);

            // Dieselben Extras lassen sich mit jedem Getränk verwenden.
            Print(new SyrupDecorator(new MilkDecorator(new BlackTea()), "Karamell"));

            // Die Reihenfolge der Dekorierer ist wichtig.
            Console.WriteLine("\nDie Reihenfolge ist wichtig:");
            Print(new LargeSizeDecorator(new SyrupDecorator(new Espresso(), "Minz")));
            Print(new SyrupDecorator(new LargeSizeDecorator(new Espresso()), "Minz"));
        }

        private static void Print(IBeverage beverage) =>
            Console.WriteLine($"{beverage.GetDescription(),-55} {beverage.GetCost():F2}");
    }
}
