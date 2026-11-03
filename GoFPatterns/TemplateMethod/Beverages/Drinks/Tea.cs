namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Classe concreta.
    public class Tea : HotBeverage
    {
        protected override void Brew() => Console.WriteLine("  lasciare in infusione la bustina di tè per 3 minuti");

        protected override void AddCondiments() => Console.WriteLine("  aggiungere limone");
    }
}
