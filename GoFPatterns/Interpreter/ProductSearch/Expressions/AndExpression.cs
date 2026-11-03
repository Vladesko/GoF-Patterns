namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Nonterminal expression - both sub-expressions must match.
    public class AndExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) && right.Matches(product);

        public override string ToString() => $"({left} AND {right})";
    }
}
