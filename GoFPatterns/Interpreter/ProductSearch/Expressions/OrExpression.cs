namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Expresión no terminal: al menos una subexpresión debe cumplirse.
    public class OrExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) || right.Matches(product);

        public override string ToString() => $"({left} OR {right})";
    }
}
