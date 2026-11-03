using GoFPatterns.Builder.PizzaShop.Builders;

namespace GoFPatterns.Builder.PizzaShop
{
    public class DemonstratePizzaBuilder
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Builder: Pizza ===\n");

            // Las mismas recetas (director) con distintos builders dan productos diferentes.
            var director = new PizzaDirector(new ItalianPizzaBuilder());
            Console.WriteLine($"Margarita italiana:   {director.MakeMargherita()}");
            Console.WriteLine($"Pepperoni italiana:   {director.MakePepperoni()}");

            director.ChangeBuilder(new AmericanPizzaBuilder());
            Console.WriteLine($"Margarita americana:  {director.MakeMargherita()}");
            Console.WriteLine($"Pepperoni americana:  {director.MakePepperoni()}");

            // El cliente puede usar el builder sin director para preparar una pizza personalizada.
            IPizzaBuilder builder = new ItalianPizzaBuilder();
            builder.SetSize("pequeña");
            builder.SetDough("centeno");
            builder.SetSauce("pesto");
            builder.AddTopping("champiñones");
            builder.AddTopping("aceitunas");
            Console.WriteLine($"\nPizza personalizada: {builder.GetResult()}");
        }
    }
}
