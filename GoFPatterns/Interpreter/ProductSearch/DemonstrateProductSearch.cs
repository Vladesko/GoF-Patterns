namespace GoFPatterns.Interpreter.ProductSearch
{
    public class DemonstrateProductSearch
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Interpreter: Suchanfragen für Produkte ===\n");

            Product[] products =
            [
                new("MacBook Air", new HashSet<string> { "laptop", "apple", "neu" }),
                new("ThinkPad T14", new HashSet<string> { "laptop", "lenovo", "gebraucht", "günstig" }),
                new("iPhone 15", new HashSet<string> { "handy", "apple", "neu" }),
                new("Galaxy S21", new HashSet<string> { "handy", "samsung", "gebraucht", "günstig" }),
                new("iPad", new HashSet<string> { "tablet", "apple", "gebraucht" })
            ];

            string[] queries =
            [
                "laptop",
                "apple AND NOT handy",
                "günstig OR neu",
                "(laptop OR tablet) AND gebraucht",
                "NOT apple AND (handy OR laptop) AND günstig"
            ];

            var parser = new QueryParser();
            foreach (var query in queries)
            {
                var expression = parser.Parse(query);
                var found = products.Where(expression.Matches).Select(p => p.Name);
                Console.WriteLine($"Anfrage: {query}");
                Console.WriteLine($"  Baum:     {expression}");
                Console.WriteLine($"  Gefunden: {string.Join(", ", found)}\n");
            }
        }
    }
}
