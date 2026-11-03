namespace GoFPatterns.Builder.PizzaShop
{
    // Produkt – komplexes Objekt, das Schritt für Schritt zusammengesetzt wird.
    public class Pizza
    {
        public string Size { get; set; } = "mittel";
        public string Dough { get; set; } = "klassischer Teig";
        public string Sauce { get; set; } = "keine";
        public List<string> Toppings { get; } = [];
        public bool ExtraCheese { get; set; }

        public override string ToString()
        {
            var toppings = Toppings.Count == 0 ? "kein Belag" : string.Join(", ", Toppings);
            var cheese = ExtraCheese ? " + extra Käse" : string.Empty;
            return $"Pizza (Größe: {Size}), {Dough}, Soße: {Sauce}, Belag: {toppings}{cheese}";
        }
    }
}
