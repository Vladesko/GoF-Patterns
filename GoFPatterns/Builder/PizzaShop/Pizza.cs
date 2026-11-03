namespace GoFPatterns.Builder.PizzaShop
{
    // Продукт - сложный объект, который собирается пошагово.
    public class Pizza
    {
        public string Size { get; set; } = "средняя";
        public string Dough { get; set; } = "классическое";
        public string Sauce { get; set; } = "нет";
        public List<string> Toppings { get; } = [];
        public bool ExtraCheese { get; set; }

        public override string ToString()
        {
            var toppings = Toppings.Count == 0 ? "нет" : string.Join(", ", Toppings);
            var cheese = ExtraCheese ? " + дополнительный сыр" : string.Empty;
            return $"пицца ({Size}), тесто: {Dough}, соус: {Sauce}, начинка: {toppings}{cheese}";
        }
    }
}
