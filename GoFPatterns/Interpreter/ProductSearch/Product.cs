namespace GoFPatterns.Interpreter.ProductSearch
{
    // Context - product which is checked by the search query.
    public record Product(string Name, IReadOnlySet<string> Tags);
}
