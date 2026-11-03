namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Expresión terminal: una sola palabra de la consulta. Se cumple si el producto tiene esa etiqueta.
    public class TagExpression(string tag) : IQueryExpression
    {
        public bool Matches(Product product) => product.Tags.Contains(tag);

        public override string ToString() => tag;
    }
}
