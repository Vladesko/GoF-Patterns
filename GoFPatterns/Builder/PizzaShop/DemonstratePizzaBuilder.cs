using GoFPatterns.Builder.PizzaShop.Builders;

namespace GoFPatterns.Builder.PizzaShop
{
    public class DemonstratePizzaBuilder
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Builder: Pizza ===\n");

            // The same recipes (director) with different builders give different products.
            var director = new PizzaDirector(new ItalianPizzaBuilder());
            Console.WriteLine($"Italian margherita: {director.MakeMargherita()}");
            Console.WriteLine($"Italian pepperoni:  {director.MakePepperoni()}");

            director.ChangeBuilder(new AmericanPizzaBuilder());
            Console.WriteLine($"American margherita: {director.MakeMargherita()}");
            Console.WriteLine($"American pepperoni:  {director.MakePepperoni()}");

            // Client can use builder without director to make a custom pizza.
            IPizzaBuilder builder = new ItalianPizzaBuilder();
            builder.SetSize("Small");
            builder.SetDough("rye");
            builder.SetSauce("pesto");
            builder.AddTopping("mushrooms");
            builder.AddTopping("olives");
            Console.WriteLine($"\nCustom pizza: {builder.GetResult()}");
        }
    }
}
