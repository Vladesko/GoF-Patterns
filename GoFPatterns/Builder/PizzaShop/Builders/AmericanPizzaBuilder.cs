namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Concrete builder - makes pizza in american style: thick dough and a lot of cheese.
    public class AmericanPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"thick {dough}";

        public void SetSauce(string sauce) => _pizza.Sauce = $"{sauce} with BBQ";

        public void AddTopping(string topping) => _pizza.Toppings.Add($"double {topping}");

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
