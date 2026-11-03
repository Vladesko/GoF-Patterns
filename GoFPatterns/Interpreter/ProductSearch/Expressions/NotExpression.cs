namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Нетерминальное выражение с одним потомком - отрицание.
    public class NotExpression(IQueryExpression inner) : IQueryExpression
    {
        public bool Matches(Product product) => !inner.Matches(product);

        public override string ToString() => $"NOT {inner}";
    }
}
