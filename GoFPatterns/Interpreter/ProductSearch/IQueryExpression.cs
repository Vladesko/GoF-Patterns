namespace GoFPatterns.Interpreter.ProductSearch
{
    // Expresión abstracta del lenguaje de consultas de búsqueda.
    public interface IQueryExpression
    {
        bool Matches(Product product);
    }
}
