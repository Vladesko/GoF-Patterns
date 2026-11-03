namespace GoFPatterns.Interpreter.ProductSearch
{
    // Contexto: producto que se comprueba con la consulta de búsqueda.
    public record Product(string Name, IReadOnlySet<string> Tags);
}
