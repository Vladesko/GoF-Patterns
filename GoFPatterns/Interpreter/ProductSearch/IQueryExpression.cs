namespace GoFPatterns.Interpreter.ProductSearch
{
    // Abstract expression of the search query language.
    public interface IQueryExpression
    {
        bool Matches(Product product);
    }
}
