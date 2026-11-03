namespace GoFPatterns.Proxy.ImageLoading
{
    // Subjekt – gemeinsame Schnittstelle für das echte Objekt und seinen Proxy.
    public interface IImage
    {
        string FileName { get; }
        void Display();
    }
}
