namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Expresión no terminal con un solo hijo: la negación.
    public class NotExpression(IQueryExpression inner) : IQueryExpression
    {
        public bool Matches(Product product) => !inner.Matches(product);

        public override string ToString() => $"NOT {inner}";
    }
}
