namespace GoFPatterns.Prototype.Documents
{
    // Prototyp – deklariert die Klonmethode.
    // Der Client kopiert Objekte über diese Schnittstelle, ohne ihre konkreten Klassen zu kennen.
    public interface IDocumentPrototype
    {
        string Title { get; set; }
        IDocumentPrototype Clone();
    }
}
