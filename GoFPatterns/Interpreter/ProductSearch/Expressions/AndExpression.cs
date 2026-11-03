namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Nichtterminaler Ausdruck – beide Teilausdrücke müssen zutreffen.
    public class AndExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) && right.Matches(product);

        public override string ToString() => $"({left} AND {right})";
    }
}
