namespace GoFPatterns.Prototype.Documents
{
    // Prototype - declares cloning method.
    // Client copies objects through this interface without knowing their concrete classes.
    public interface IDocumentPrototype
    {
        string Title { get; set; }
        IDocumentPrototype Clone();
    }
}
