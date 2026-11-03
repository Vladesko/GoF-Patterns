using GoFPatterns.TemplateMethod.Beverages.Drinks;

namespace GoFPatterns.TemplateMethod.Beverages
{
    public class DemonstrateBeverages
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Шаблонный метод: горячие напитки ===\n");

            HotBeverage[] order = [new Tea(), new GreenTea(), new Coffee(withMilk: true), new Coffee(withMilk: false)];
            foreach (var beverage in order)
            {
                beverage.Prepare();
            }
        }
    }
}
