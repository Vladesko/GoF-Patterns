namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Нетерминальное выражение - должно совпасть хотя бы одно подвыражение.
    public class OrExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) || right.Matches(product);

        public override string ToString() => $"({left} OR {right})";
    }
}
