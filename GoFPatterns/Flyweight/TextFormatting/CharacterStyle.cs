namespace GoFPatterns.Flyweight.TextFormatting
{
    // Flyweight – Formatierungsstil, den sich viele Zeichen teilen.
    // Ein Record ist unveränderlich und wird nach Wert verglichen, daher taugt er als Dictionary-Schlüssel.
    public sealed record CharacterStyle(string Font, int Size, bool Bold, string Color);
}
