namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Builder concreto: prepara la pizza al estilo americano, con masa gruesa y mucho queso.
    public class AmericanPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"gruesa de {dough}";

        public void SetSauce(string sauce) => _pizza.Sauce = $"{sauce} con BBQ";

        public void AddTopping(string topping) => _pizza.Toppings.Add($"doble de {topping}");

        public void AddExtraCheese()
        {
            _pizza.ExtraCheese = true;
            _pizza.Toppings.Add("cheddar");
        }

        public Pizza GetResult()
        {
            var result = _pizza;
            Reset();
            return result;
        }
    }
}
