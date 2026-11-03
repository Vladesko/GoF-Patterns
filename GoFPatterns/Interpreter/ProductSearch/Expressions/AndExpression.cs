namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Espressione non terminale - entrambe le sotto-espressioni devono corrispondere.
    public class AndExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) && right.Matches(product);

        public override string ToString() => $"({left} AND {right})";
    }
}
