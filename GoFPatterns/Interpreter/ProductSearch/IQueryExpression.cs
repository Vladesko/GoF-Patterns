namespace GoFPatterns.Interpreter.ProductSearch
{
    // Абстрактное выражение языка поисковых запросов.
    public interface IQueryExpression
    {
        bool Matches(Product product);
    }
}
