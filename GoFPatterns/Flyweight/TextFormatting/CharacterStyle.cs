namespace GoFPatterns.Flyweight.TextFormatting
{
    // Легковес - стиль форматирования, который разделяют многие символы.
    // Record неизменяем и сравнивается по значению, поэтому его можно использовать как ключ словаря.
    public sealed record CharacterStyle(string Font, int Size, bool Bold, string Color);
}
