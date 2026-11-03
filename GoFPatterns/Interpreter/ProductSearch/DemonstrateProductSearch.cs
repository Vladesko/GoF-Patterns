namespace GoFPatterns.Interpreter.ProductSearch
{
    public class DemonstrateProductSearch
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Interpreter: query di ricerca prodotti ===\n");

            Product[] products =
            [
                new("MacBook Air", new HashSet<string> { "portatile", "apple", "nuovo" }),
                new("ThinkPad T14", new HashSet<string> { "portatile", "lenovo", "usato", "economico" }),
                new("iPhone 15", new HashSet<string> { "telefono", "apple", "nuovo" }),
                new("Galaxy S21", new HashSet<string> { "telefono", "samsung", "usato", "economico" }),
                new("iPad", new HashSet<string> { "tablet", "apple", "usato" })
            ];

            string[] queries =
            [
                "portatile",
                "apple AND NOT telefono",
                "economico OR nuovo",
                "(portatile OR tablet) AND usato",
                "NOT apple AND (telefono OR portatile) AND economico"
            ];

            var parser = new QueryParser();
            foreach (var query in queries)
            {
                var expression = parser.Parse(query);
                var found = products.Where(expression.Matches).Select(p => p.Name);
                Console.WriteLine($"Query:   {query}");
                Console.WriteLine($"  Albero:  {expression}");
                Console.WriteLine($"  Trovati: {string.Join(", ", found)}\n");
            }
        }
    }
}
