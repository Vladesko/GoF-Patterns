namespace GoFPatterns.Builder.PizzaShop
{
    // Producto: objeto complejo que se ensambla paso a paso.
    public class Pizza
    {
        public string Size { get; set; } = "mediana";
        public string Dough { get; set; } = "clásica";
        public string Sauce { get; set; } = "ninguna";
        public List<string> Toppings { get; } = [];
        public bool ExtraCheese { get; set; }

        public override string ToString()
        {
            var toppings = Toppings.Count == 0 ? "sin ingredientes" : string.Join(", ", Toppings);
            var cheese = ExtraCheese ? " + queso extra" : string.Empty;
            return $"Pizza {Size} con masa {Dough}, salsa: {Sauce}, ingredientes: {toppings}{cheese}";
        }
    }
}
