namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Konkreter Builder – backt Pizza nach italienischer Art: dünner Teig und standardmäßig Tomatensoße.
    public class ItalianPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"dünner {dough}";

        public void SetSauce(string sauce) => _pizza.Sauce = sauce;

        public void AddTopping(string topping) => _pizza.Toppings.Add(topping);

        public void AddExtraCheese()
        {
            _pizza.ExtraCheese = true;
            _pizza.Toppings.Add("Parmesan");
        }

        // Der Builder gibt das Ergebnis zurück und ist bereit für die nächste Pizza.
        public Pizza GetResult()
        {
            var result = _pizza;
            Reset();
            return result;
        }
    }
}
