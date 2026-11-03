namespace GoFPatterns.Flyweight.TextFormatting
{
    // Kontext – ein Zeichen des Dokuments. Symbol und Position sind extrinsischer Zustand,
    // der Stil ist eine Referenz auf ein gemeinsam genutztes Flyweight.
    public readonly record struct FormattedCharacter(char Symbol, int Position, CharacterStyle Style);
}
