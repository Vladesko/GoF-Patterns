namespace GoFPatterns.Prototype.Documents
{
    // Prototype - dichiara il metodo di clonazione.
    // Il client copia gli oggetti tramite questa interfaccia senza conoscerne le classi concrete.
    public interface IDocumentPrototype
    {
        string Title { get; set; }
        IDocumentPrototype Clone();
    }
}
