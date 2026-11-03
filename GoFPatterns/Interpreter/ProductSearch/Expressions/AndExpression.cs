namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Нетерминальное выражение - должны совпасть оба подвыражения.
    public class AndExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) && right.Matches(product);

        public override string ToString() => $"({left} AND {right})";
    }
}
