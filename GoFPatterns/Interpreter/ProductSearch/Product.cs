namespace GoFPatterns.Interpreter.ProductSearch
{
    // Контекст - товар, который проверяется поисковым запросом.
    public record Product(string Name, IReadOnlySet<string> Tags);
}
