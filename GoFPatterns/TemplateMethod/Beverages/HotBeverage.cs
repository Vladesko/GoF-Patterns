namespace GoFPatterns.TemplateMethod.Beverages
{
    // Clase abstracta: receta de cualquier bebida caliente.
    public abstract class HotBeverage
    {
        // Método plantilla.
        public void Prepare()
        {
            Console.WriteLine($"Preparando {GetType().Name}:");
            BoilWater();
            Brew();
            PourInCup();
            if (CustomerWantsCondiments())
                AddCondiments();
            Console.WriteLine();
        }

        // Pasos comunes.
        private void BoilWater() => Console.WriteLine($"  hervir el agua a {WaterTemperature}°C");
        private static void PourInCup() => Console.WriteLine("  verter en la taza");

        // Pasos implementados por las subclases.
        protected abstract void Brew();
        protected abstract void AddCondiments();

        // Hooks: las subclases pueden sobrescribirlos, pero no están obligadas.
        protected virtual int WaterTemperature => 100;
        protected virtual bool CustomerWantsCondiments() => true;
    }
}
