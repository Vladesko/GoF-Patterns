using GoFPatterns.Builder.PizzaShop.Builders;

namespace GoFPatterns.Builder.PizzaShop
{
    public class DemonstratePizzaBuilder
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Строитель: пицца ===\n");

            // Одни и те же рецепты (распорядитель) с разными строителями дают разные продукты.
            var director = new PizzaDirector(new ItalianPizzaBuilder());
            Console.WriteLine($"Итальянская маргарита:  {director.MakeMargherita()}");
            Console.WriteLine($"Итальянская пепперони:  {director.MakePepperoni()}");

            director.ChangeBuilder(new AmericanPizzaBuilder());
            Console.WriteLine($"Американская маргарита: {director.MakeMargherita()}");
            Console.WriteLine($"Американская пепперони: {director.MakePepperoni()}");

            // Клиент может использовать строитель без распорядителя, чтобы собрать свою пиццу.
            IPizzaBuilder builder = new ItalianPizzaBuilder();
            builder.SetSize("маленькая");
            builder.SetDough("ржаное");
            builder.SetSauce("песто");
            builder.AddTopping("грибы");
            builder.AddTopping("оливки");
            Console.WriteLine($"\nСвоя пицца: {builder.GetResult()}");
        }
    }
}
