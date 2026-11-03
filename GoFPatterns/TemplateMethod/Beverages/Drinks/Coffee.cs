namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Konkrete Klasse mit einem Hook, der von der Wahl des Kunden abhängt.
    public class Coffee(bool withMilk) : HotBeverage
    {
        protected override int WaterTemperature => 92;

        protected override void Brew() => Console.WriteLine("  Kaffee durch den Filter laufen lassen");

        protected override void AddCondiments() => Console.WriteLine("  Zucker und Milch hinzufügen");

        protected override bool CustomerWantsCondiments() => withMilk;
    }
}
