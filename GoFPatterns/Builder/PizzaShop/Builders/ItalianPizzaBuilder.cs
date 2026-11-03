namespace GoFPatterns.Builder.PizzaShop.Builders
{
    // Builder concreto: prepara la pizza al estilo italiano, con masa fina y salsa de tomate por defecto.
    public class ItalianPizzaBuilder : IPizzaBuilder
    {
        private Pizza _pizza = new();

        public void Reset() => _pizza = new Pizza();

        public void SetSize(string size) => _pizza.Size = size;

        public void SetDough(string dough) => _pizza.Dough = $"fina de {dough}";

        public void SetSauce(string sauce) => _pizza.Sauce = sauce;

        public void AddTopping(string topping) => _pizza.Toppings.Add(topping);

        public void AddExtraCheese()
        {
            _pizza.ExtraCheese = true;
            _pizza.Toppings.Add("parmesano");
        }

        // El builder devuelve el resultado y queda listo para construir la siguiente pizza.
        public Pizza GetResult()
        {
            var result = _pizza;
            Reset();
            return result;
        }
    }
}
