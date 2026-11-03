namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Конкретный строитель - готовит пиццу в американском стиле: толстое тесто и много сыра.
    public class AmericanPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"толстое {dough}";

        public void SetSauce(string sauce) => _pizza.Sauce = $"{sauce} с BBQ";

        public void AddTopping(string topping) => _pizza.Toppings.Add($"{topping} x2");

        public void AddExtraCheese()
        {
            _pizza.ExtraCheese = true;
            _pizza.Toppings.Add("чеддер");
        }

        public Pizza GetResult()
        {
            var result = _pizza;
            Reset();
            return result;
        }
    }
}
