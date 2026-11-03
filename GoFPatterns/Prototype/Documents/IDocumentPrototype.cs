namespace GoFPatterns.Prototype.Documents
{
    // Prototype: declara el método de clonación.
    // El cliente copia objetos a través de esta interfaz sin conocer sus clases concretas.
    public interface IDocumentPrototype
    {
        string Title { get; set; }
        IDocumentPrototype Clone();
    }
}
