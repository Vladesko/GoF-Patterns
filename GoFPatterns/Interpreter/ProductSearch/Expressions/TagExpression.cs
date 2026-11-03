namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Espressione terminale - una singola parola della query. Corrisponde se il prodotto ha quel tag.
    public class TagExpression(string tag) : IQueryExpression
    {
        public bool Matches(Product product) => product.Tags.Contains(tag);

        public override string ToString() => tag;
    }
}
