namespace GoFPatterns.Interpreter.ProductSearch
{
    // Context - prodotto che viene verificato dalla query di ricerca.
    public record Product(string Name, IReadOnlySet<string> Tags);
}
