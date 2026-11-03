namespace GoFPatterns.Decorator.Coffee.AddOns
{
    // Decorator concreto che modifica il risultato dell'oggetto avvolto invece di aggiungervi qualcosa.
    // L'ordine dei decorator conta: il formato grande moltiplica il costo di tutto ciò che contiene.
    public class LargeSizeDecorator(IBeverage beverage) : BeverageDecorator(beverage)
    {
        public override string GetDescription() => $"Grande ({base.GetDescription()})";

        public override decimal GetCost() => base.GetCost() * 1.5m;
    }
}
