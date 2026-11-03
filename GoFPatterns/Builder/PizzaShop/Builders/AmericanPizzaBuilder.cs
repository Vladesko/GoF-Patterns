namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Konkreter Builder – backt Pizza nach amerikanischer Art: dicker Teig und viel Käse.
    public class AmericanPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"dicker {dough}";

        public void SetSauce(string sauce) => _pizza.Sauce = $"{sauce} mit BBQ";

        public void AddTopping(string topping) => _pizza.Toppings.Add($"doppelt {topping}");

        public void AddExtraCheese()
        {
            _pizza.ExtraCheese = true;
            _pizza.Toppings.Add("Cheddar");
        }

        public Pizza GetResult()
        {
            var result = _pizza;
            Reset();
            return result;
        }
    }
}
