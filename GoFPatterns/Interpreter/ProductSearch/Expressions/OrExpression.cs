namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Espressione non terminale - almeno una sotto-espressione deve corrispondere.
    public class OrExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) || right.Matches(product);

        public override string ToString() => $"({left} OR {right})";
    }
}
