namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Clase concreta con un hook que depende de la elección del cliente.
    public class Coffee(bool withMilk) : HotBeverage
    {
        protected override int WaterTemperature => 92;

        protected override void Brew() => Console.WriteLine("  pasar el café por el filtro");

        protected override void AddCondiments() => Console.WriteLine("  añadir azúcar y leche");

        protected override bool CustomerWantsCondiments() => withMilk;
    }
}
