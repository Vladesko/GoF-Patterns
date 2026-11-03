namespace GoFPatterns.Proxy.ImageLoading
{
    // Sujeto: interfaz común para el objeto real y su proxy.
    public interface IImage
    {
        string FileName { get; }
        void Display();
    }
}
