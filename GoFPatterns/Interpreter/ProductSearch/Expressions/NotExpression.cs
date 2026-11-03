namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Espressione non terminale con un solo figlio - la negazione.
    public class NotExpression(IQueryExpression inner) : IQueryExpression
    {
        public bool Matches(Product product) => !inner.Matches(product);

        public override string ToString() => $"NOT {inner}";
    }
}
