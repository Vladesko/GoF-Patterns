namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Konkreter Dekorierer, der das Ergebnis des umhüllten Objekts verändert, statt etwas hinzuzufügen.
    // Die Reihenfolge der Dekorierer ist wichtig: Die große Größe vervielfacht die Kosten von allem, was darin steckt.
    public class LargeSizeDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"Groß ({base.GetDescription()})";

        public override decimal GetCost() => base.GetCost() * 1.5m;
    }
}
