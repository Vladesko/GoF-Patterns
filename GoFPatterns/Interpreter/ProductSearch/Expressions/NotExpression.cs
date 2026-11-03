namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Nichtterminaler Ausdruck mit einem Kind – Negation.
    public class NotExpression(IQueryExpression inner) : IQueryExpression
    {
        public bool Matches(Product product) => !inner.Matches(product);

        public override string ToString() => $"NOT {inner}";
    }
}
