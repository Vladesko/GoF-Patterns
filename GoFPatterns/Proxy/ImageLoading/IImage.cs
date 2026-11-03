namespace GoFPatterns.Proxy.ImageLoading
{
    // Subject - interfaccia comune per l'oggetto reale e il suo proxy.
    public interface IImage
    {
        string FileName { get; }
        void Display();
    }
}
