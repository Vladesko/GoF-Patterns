namespace GoFPatterns.Builder.PizzaShop
{
    // Product - oggetto complesso che viene assemblato passo dopo passo.
    public class Pizza
    {
        public string Size { get; set; } = "media";
        public string Dough { get; set; } = "classico";
        public string Sauce { get; set; } = "nessuna";
        public List<string> Toppings { get; } = [];
        public bool ExtraCheese { get; set; }

        public override string ToString()
        {
            var toppings = Toppings.Count == 0 ? "nessun condimento" : string.Join(", ", Toppings);
            var cheese = ExtraCheese ? " + formaggio extra" : string.Empty;
            return $"Pizza {Size}, impasto: {Dough}, salsa: {Sauce}, condimenti: {toppings}{cheese}";
        }
    }
}
