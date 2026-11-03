namespace GoFPatterns.TemplateMethod.Beverages
{
    // Abstrakte Klasse – Rezept für jedes Heißgetränk.
    public abstract class HotBeverage
    {
        // Schablonenmethode (Template Method).
        public void Prepare()
        {
            Console.WriteLine($"Zubereitung: {GetType().Name}");
            BoilWater();
            Brew();
            PourInCup();
            if (CustomerWantsCondiments())
                AddCondiments();
            Console.WriteLine();
        }

        // Gemeinsame Schritte.
        private void BoilWater() => Console.WriteLine($"  Wasser auf {WaterTemperature} °C erhitzen");
        private static void PourInCup() => Console.WriteLine("  in die Tasse gießen");

        // Schritte, die von Unterklassen implementiert werden.
        protected abstract void Brew();
        protected abstract void AddCondiments();

        // Hooks – Unterklassen dürfen sie überschreiben, müssen es aber nicht.
        protected virtual int WaterTemperature => 100;
        protected virtual bool CustomerWantsCondiments() => true;
    }
}
