namespace GoFPatterns.Builder.PizzaShop
{
    // Product - complex object which is assembled step by step.
    public class Pizza
    {
        public string Size { get; set; } = "Medium";
        public string Dough { get; set; } = "Classic";
        public string Sauce { get; set; } = "None";
        public List<string> Toppings { get; } = [];
        public bool ExtraCheese { get; set; }

        public override string ToString()
        {
            var toppings = Toppings.Count == 0 ? "no toppings" : string.Join(", ", Toppings);
            var cheese = ExtraCheese ? " + extra cheese" : string.Empty;
            return $"{Size} pizza on {Dough} dough, sauce: {Sauce}, toppings: {toppings}{cheese}";
        }
    }
}
