namespace GoFPatterns.Interpreter.ProductSearch
{
    // Abstrakter Ausdruck der Suchanfragesprache.
    public interface IQueryExpression
    {
        bool Matches(Product product);
    }
}
