namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Конкретный строитель - готовит пиццу в итальянском стиле: тонкое тесто и томатный соус по умолчанию.
    public class ItalianPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"тонкое {dough}";

        public void SetSauce(string sauce) => _pizza.Sauce = sauce;

        public void AddTopping(string topping) => _pizza.Toppings.Add(topping);

        public void AddExtraCheese()
        {
            _pizza.ExtraCheese = true;
            _pizza.Toppings.Add("пармезан");
        }

        // Строитель возвращает результат и готов собирать следующую пиццу.
        public Pizza GetResult()
        {
            var result = _pizza;
            Reset();
            return result;
        }
    }
}
