namespace GoFPatterns.TemplateMethod.Beverages
{
    // Abstract class - recipe of any hot beverage.
    public abstract class HotBeverage
    {
        // Template method.
        public void Prepare()
        {
            Console.WriteLine($"Preparing {GetType().Name}:");
            BoilWater();
            Brew();
            PourInCup();
            if (CustomerWantsCondiments())
                AddCondiments();
            Console.WriteLine();
        }

        // Common steps.
        private void BoilWater() => Console.WriteLine($"  boil water to {WaterTemperature}°C");
        private static void PourInCup() => Console.WriteLine("  pour into cup");

        // Steps implemented by subclasses.
        protected abstract void Brew();
        protected abstract void AddCondiments();

        // Hooks - subclasses may override them, but don't have to.
        protected virtual int WaterTemperature => 100;
        protected virtual bool CustomerWantsCondiments() => true;
    }
}
