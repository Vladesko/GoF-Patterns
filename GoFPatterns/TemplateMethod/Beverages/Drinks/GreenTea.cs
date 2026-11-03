namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Classe concreta che usa gli hook: temperatura dell'acqua più bassa e nessun condimento.
    public class GreenTea : HotBeverage
    {
        protected override int WaterTemperature => 80;

        protected override void Brew() => Console.WriteLine("  lasciare in infusione le foglie di tè verde per 2 minuti");

        protected override void AddCondiments() => Console.WriteLine("  aggiungere gelsomino");

        protected override bool CustomerWantsCondiments() => false;
    }
}
