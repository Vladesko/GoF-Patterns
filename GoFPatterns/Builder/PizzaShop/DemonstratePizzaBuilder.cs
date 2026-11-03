using GoFPatterns.Builder.PizzaShop.Builders;

namespace GoFPatterns.Builder.PizzaShop
{
    public class DemonstratePizzaBuilder
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Builder: Pizza ===\n");

            // Le stesse ricette (director) con builder diversi danno prodotti diversi.
            var director = new PizzaDirector(new ItalianPizzaBuilder());
            Console.WriteLine($"Margherita italiana: {director.MakeMargherita()}");
            Console.WriteLine($"Pepperoni italiana:  {director.MakePepperoni()}");

            director.ChangeBuilder(new AmericanPizzaBuilder());
            Console.WriteLine($"Margherita americana: {director.MakeMargherita()}");
            Console.WriteLine($"Pepperoni americana:  {director.MakePepperoni()}");

            // Il client può usare il builder senza director per creare una pizza personalizzata.
            IPizzaBuilder builder = new ItalianPizzaBuilder();
            builder.SetSize("piccola");
            builder.SetDough("segale");
            builder.SetSauce("pesto");
            builder.AddTopping("funghi");
            builder.AddTopping("olive");
            Console.WriteLine($"\nPizza personalizzata: {builder.GetResult()}");
        }
    }
}
