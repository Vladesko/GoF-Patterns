namespace GoFPatterns.Interpreter.ProductSearch.Expressions
{
    // Terminaler Ausdruck – ein einzelnes Wort der Anfrage. Trifft zu, wenn das Produkt dieses Tag hat.
    public class TagExpression(string tag) : IQueryExpression
    {
        public bool Matches(Product product) => product.Tags.Contains(tag);

        public override string ToString() => tag;
    }
}
