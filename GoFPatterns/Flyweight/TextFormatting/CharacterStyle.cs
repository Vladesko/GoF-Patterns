namespace GoFPatterns.Flyweight.TextFormatting
{
    // Flyweight - stile di formattazione condiviso da molti caratteri.
    // Il record è immutabile e si confronta per valore, quindi può essere usato come chiave di un dizionario.
    public sealed record CharacterStyle(string Font, int Size, bool Bold, string Color);
}
