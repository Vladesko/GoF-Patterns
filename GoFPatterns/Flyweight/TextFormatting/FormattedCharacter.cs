namespace GoFPatterns.Flyweight.TextFormatting
{
    // Contexto: un carácter del documento. El símbolo y la posición son estado extrínseco;
    // el estilo es una referencia al flyweight compartido.
    public readonly record struct FormattedCharacter(char Symbol, int Position, CharacterStyle Style);
}
