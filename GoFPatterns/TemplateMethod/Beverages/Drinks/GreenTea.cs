namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Clase concreta que usa hooks: temperatura del agua más baja y sin complementos.
    public class GreenTea : HotBeverage
    {
        protected override int WaterTemperature => 80;

        protected override void Brew() => Console.WriteLine("  infusionar las hojas de té verde durante 2 minutos");

        protected override void AddCondiments() => Console.WriteLine("  añadir jazmín");

        protected override bool CustomerWantsCondiments() => false;
    }
}
