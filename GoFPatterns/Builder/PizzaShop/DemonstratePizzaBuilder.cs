using GoFPatterns.Builder.PizzaShop.Builders;

namespace GoFPatterns.Builder.PizzaShop
{
    public class DemonstratePizzaBuilder
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Builder: Pizza ===\n");

            // Dieselben Rezepte (Director) ergeben mit verschiedenen Buildern verschiedene Produkte.
            var director = new PizzaDirector(new ItalianPizzaBuilder());
            Console.WriteLine($"Italienische Margherita: {director.MakeMargherita()}");
            Console.WriteLine($"Italienische Pepperoni:  {director.MakePepperoni()}");

            director.ChangeBuilder(new AmericanPizzaBuilder());
            Console.WriteLine($"Amerikanische Margherita: {director.MakeMargherita()}");
            Console.WriteLine($"Amerikanische Pepperoni:  {director.MakePepperoni()}");

            // Der Client kann den Builder auch ohne Director nutzen, um eine individuelle Pizza zu bauen.
            IPizzaBuilder builder = new ItalianPizzaBuilder();
            builder.SetSize("klein");
            builder.SetDough("Roggenteig");
            builder.SetSauce("Pesto");
            builder.AddTopping("Champignons");
            builder.AddTopping("Oliven");
            Console.WriteLine($"\nIndividuelle Pizza: {builder.GetResult()}");
        }
    }
}
