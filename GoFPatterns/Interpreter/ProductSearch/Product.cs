namespace GoFPatterns.Interpreter.ProductSearch
{
    // Kontext – das Produkt, das von der Suchanfrage geprüft wird.
    public record Product(string Name, IReadOnlySet<string> Tags);
}
