using GoFPatterns.Decorator.Coffee.AddOns;
using GoFPatterns.Decorator.Coffee.Beverages;

namespace GoFPatterns.Decorator.Coffee
{
    public class DemonstrateCoffee
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Декоратор: кофейня ===\n");

            IBeverage espresso = new Espresso();
            Print(espresso);

            // Декораторы оборачивают друг друга, как матрёшка.
            IBeverage latte = new MilkDecorator(new MilkDecorator(new Espresso()));
            Print(latte);

            IBeverage vanillaLatte = new SyrupDecorator(latte, "ванильный");
            Print(vanillaLatte);

            IBeverage largeVanillaLatte = new LargeSizeDecorator(vanillaLatte);
            Print(largeVanillaLatte);

            // Те же добавки можно использовать с любым напитком.
            Print(new SyrupDecorator(new MilkDecorator(new BlackTea()), "карамельный"));

            // Порядок декораторов важен.
            Console.WriteLine("\nПорядок имеет значение:");
            Print(new LargeSizeDecorator(new SyrupDecorator(new Espresso(), "мятный")));
            Print(new SyrupDecorator(new LargeSizeDecorator(new Espresso()), "мятный"));
        }

        private static void Print(IBeverage beverage) =>
            Console.WriteLine($"{beverage.GetDescription(),-55} {beverage.GetCost():F2}");
    }
}
