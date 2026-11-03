namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Терминальное выражение - одно слово запроса. Совпадает, если у товара есть такой тег.
    public class TagExpression(string tag) : IQueryExpression
    {
        public bool Matches(Product product) => product.Tags.Contains(tag);

        public override string ToString() => tag;
    }
}
