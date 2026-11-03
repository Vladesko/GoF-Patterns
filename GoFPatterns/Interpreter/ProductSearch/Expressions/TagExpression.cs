namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Terminal expression - single word of the query. Matches if product has such tag.
    public class TagExpression(string tag) : IQueryExpression
    {
        public bool Matches(Product product) => product.Tags.Contains(tag);

        public override string ToString() => tag;
    }
}
