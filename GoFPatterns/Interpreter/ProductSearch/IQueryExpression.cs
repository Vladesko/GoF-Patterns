namespace GoFPatterns.Interpreter.ProductSearch
{
    // Espressione astratta del linguaggio delle query di ricerca.
    public interface IQueryExpression
    {
        bool Matches(Product product);
    }
}
