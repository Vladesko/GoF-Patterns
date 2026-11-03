namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Builder concreto - prepara la pizza in stile italiano: impasto sottile e salsa di pomodoro di default.
    public class ItalianPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"{dough} (sottile)";

        public void SetSauce(string sauce) => _pizza.Sauce = sauce;

        public void AddTopping(string topping) => _pizza.Toppings.Add(topping);

        public void AddExtraCheese()
        {
            _pizza.ExtraCheese = true;
            _pizza.Toppings.Add("parmigiano");
        }

        // Il builder restituisce il risultato ed è pronto a costruire la pizza successiva.
        public Pizza GetResult()
        {
            var result = _pizza;
            Reset();
            return result;
        }
    }
}
