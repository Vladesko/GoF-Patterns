namespace GoFPatterns.Interpreter.ProductSearch
{
    public class DemonstrateProductSearch
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Интерпретатор: поисковые запросы по товарам ===\n");

            Product[] products =
            [
                new("MacBook Air", new HashSet<string> { "laptop", "apple", "new" }),
                new("ThinkPad T14", new HashSet<string> { "laptop", "lenovo", "used", "cheap" }),
                new("iPhone 15", new HashSet<string> { "phone", "apple", "new" }),
                new("Galaxy S21", new HashSet<string> { "phone", "samsung", "used", "cheap" }),
                new("iPad", new HashSet<string> { "tablet", "apple", "used" })
            ];

            string[] queries =
            [
                "laptop",
                "apple AND NOT phone",
                "cheap OR new",
                "(laptop OR tablet) AND used",
                "NOT apple AND (phone OR laptop) AND cheap"
            ];

            var parser = new QueryParser();
            foreach (var query in queries)
            {
                var expression = parser.Parse(query);
                var found = products.Where(expression.Matches).Select(p => p.Name);
                Console.WriteLine($"Запрос:  {query}");
                Console.WriteLine($"  Дерево:  {expression}");
                Console.WriteLine($"  Найдено: {string.Join(", ", found)}\n");
            }
        }
    }
}
