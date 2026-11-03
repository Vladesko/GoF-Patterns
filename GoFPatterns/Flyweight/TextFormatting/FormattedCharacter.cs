namespace GoFPatterns.Flyweight.TextFormatting
{
    // Контекст - один символ документа. Символ и позиция - внешнее состояние,
    // стиль - ссылка на разделяемый легковес.
    public readonly record struct FormattedCharacter(char Symbol, int Position, CharacterStyle Style);
}
