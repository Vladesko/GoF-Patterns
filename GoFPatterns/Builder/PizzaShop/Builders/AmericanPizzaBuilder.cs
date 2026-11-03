namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Builder concreto - prepara la pizza in stile americano: impasto alto e tanto formaggio.
    public class AmericanPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"{dough} (alto)";

        public void SetSauce(string sauce) => _pizza.Sauce = $"{sauce} con BBQ";

        public void AddTopping(string topping) => _pizza.Toppings.Add($"{topping} x2");

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
