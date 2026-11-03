namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Nichtterminaler Ausdruck – mindestens ein Teilausdruck muss zutreffen.
    public class OrExpression(IQueryExpression left, IQueryExpression right) : IQueryExpression
    {
        public bool Matches(Product product) => left.Matches(product) || right.Matches(product);

        public override string ToString() => $"({left} OR {right})";
    }
}
