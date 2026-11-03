namespace GoFPatterns.Flyweight.TextFormatting
{
    // Flyweight: estilo de formato compartido por muchos caracteres.
    // Un record es inmutable y se compara por valor, así que se puede usar como clave de diccionario.
    public sealed record CharacterStyle(string Font, int Size, bool Bold, string Color);
}
