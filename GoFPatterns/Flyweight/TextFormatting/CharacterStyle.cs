namespace GoFPatterns.Flyweight.TextFormatting
{
    // Flyweight - formatting style which is shared by many characters.
    // Record is immutable and compares by value, so it can be used as a dictionary key.
    public sealed record CharacterStyle(string Font, int Size, bool Bold, string Color);
}
