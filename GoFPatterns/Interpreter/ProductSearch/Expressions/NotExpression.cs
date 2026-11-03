namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Nonterminal expression with one child - negation.
    public class NotExpression(IQueryExpression inner) : IQueryExpression
    {
        public bool Matches(Product product) => !inner.Matches(product);

        public override string ToString() => $"NOT {inner}";
    }
}
