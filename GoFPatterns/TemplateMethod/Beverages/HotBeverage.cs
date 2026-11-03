namespace GoFPatterns.TemplateMethod.Beverages
{
    // Абстрактный класс - рецепт любого горячего напитка.
    public abstract class HotBeverage
    {
        // Шаблонный метод.
        public void Prepare()
        {
            Console.WriteLine($"Готовим {GetType().Name}:");
            BoilWater();
            Brew();
            PourInCup();
            if (CustomerWantsCondiments())
                AddCondiments();
            Console.WriteLine();
        }

        // Общие шаги.
        private void BoilWater() => Console.WriteLine($"  вскипятить воду до {WaterTemperature}°C");
        private static void PourInCup() => Console.WriteLine("  налить в чашку");

        // Шаги, которые реализуют подклассы.
        protected abstract void Brew();
        protected abstract void AddCondiments();

        // Хуки - подклассы могут их переопределить, но не обязаны.
        protected virtual int WaterTemperature => 100;
        protected virtual bool CustomerWantsCondiments() => true;
    }
}
