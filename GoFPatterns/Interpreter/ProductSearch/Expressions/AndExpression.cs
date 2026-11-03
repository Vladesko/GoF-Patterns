namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Expresión no terminal: ambas subexpresiones deben cumplirse.
    public class AndExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) && right.Matches(product);

        public override string ToString() => $"({left} AND {right})";
    }
}
