namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Nonterminal expression - at least one sub-expression must match.
    public class OrExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) || right.Matches(product);

        public override string ToString() => $"({left} OR {right})";
    }
}
