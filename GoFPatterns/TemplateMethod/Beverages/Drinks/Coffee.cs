namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Classe concreta con un hook che dipende dalla scelta del cliente.
    public class Coffee(bool withMilk) : HotBeverage
    {
        protected override int WaterTemperature => 92;

        protected override void Brew() => Console.WriteLine("  far passare il caffè attraverso il filtro");

        protected override void AddCondiments() => Console.WriteLine("  aggiungere zucchero e latte");

        protected override bool CustomerWantsCondiments() => withMilk;
    }
}
