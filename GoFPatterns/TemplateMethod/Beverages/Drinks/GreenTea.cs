namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Konkrete Klasse, die Hooks nutzt: niedrigere Wassertemperatur und keine Zutaten.
    public class GreenTea : HotBeverage
    {
        protected override int WaterTemperature => 80;

        protected override void Brew() => Console.WriteLine("  grüne Teeblätter 2 Minuten ziehen lassen");

        protected override void AddCondiments() => Console.WriteLine("  Jasmin hinzufügen");

        protected override bool CustomerWantsCondiments() => false;
    }
}
