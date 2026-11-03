namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Concrete builder - makes pizza in italian style: thin dough and tomato sauce by default.
    public class ItalianPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"thin {dough}";

        public void SetSauce(string sauce) => _pizza.Sauce = sauce;

        public void AddTopping(string topping) => _pizza.Toppings.Add(topping);

        public void AddExtraCheese()
        {
            _pizza.ExtraCheese = true;
            _pizza.Toppings.Add("parmesan");
        }

        // Builder returns result and is ready to build the next pizza.
        public Pizza GetResult()
        {
            var result = _pizza;
            Reset();
            return result;
        }
    }
}
