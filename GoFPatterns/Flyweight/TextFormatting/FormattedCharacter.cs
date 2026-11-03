namespace GoFPatterns.Flyweight.TextFormatting
{
    // Context - un carattere del documento. Il simbolo e la posizione sono stato estrinseco,
    // lo stile è un riferimento al flyweight condiviso.
    public readonly record struct FormattedCharacter(char Symbol, int Position, CharacterStyle Style);
}
