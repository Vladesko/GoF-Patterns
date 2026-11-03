namespace GoFPatterns.Flyweight.TextFormatting
{
    // Context - one character of document. Symbol and position are extrinsic state,
    // style is a reference to shared flyweight.
    public readonly record struct FormattedCharacter(char Symbol, int Position, CharacterStyle Style);
}
