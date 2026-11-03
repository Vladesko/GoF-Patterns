namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Konkrete Klasse.
    public class Tea : HotBeverage
    {
        protected override void Brew() => Console.WriteLine("  Teebeutel 3 Minuten ziehen lassen");

        protected override void AddCondiments() => Console.WriteLine("  Zitrone hinzufügen");
    }
}
