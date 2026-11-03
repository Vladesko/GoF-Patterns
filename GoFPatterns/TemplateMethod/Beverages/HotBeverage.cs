namespace GoFPatterns.TemplateMethod.Beverages
{
    // Classe astratta - la ricetta di qualsiasi bevanda calda.
    public abstract class HotBeverage
    {
        // Template method.
        public void Prepare()
        {
            Console.WriteLine($"Preparazione di {GetType().Name}:");
            BoilWater();
            Brew();
            PourInCup();
            if (CustomerWantsCondiments())
                AddCondiments();
            Console.WriteLine();
        }

        // Passi comuni.
        private void BoilWater() => Console.WriteLine($"  scaldare l'acqua a {WaterTemperature}°C");
        private static void PourInCup() => Console.WriteLine("  versare nella tazza");

        // Passi implementati dalle sottoclassi.
        protected abstract void Brew();
        protected abstract void AddCondiments();

        // Hook - le sottoclassi possono ridefinirli, ma non sono obbligate.
        protected virtual int WaterTemperature => 100;
        protected virtual bool CustomerWantsCondiments() => true;
    }
}
