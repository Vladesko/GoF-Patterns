namespace GoFPatterns.TemplateMethod.Beverages.Drinks
{
    // Clase concreta.
    public class Tea : HotBeverage
    {
        protected override void Brew() => Console.WriteLine("  infusionar la bolsita de té durante 3 minutos");

        protected override void AddCondiments() => Console.WriteLine("  añadir limón");
    }
}
