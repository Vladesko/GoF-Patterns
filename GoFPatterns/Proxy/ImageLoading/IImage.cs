namespace GoFPatterns.Proxy.ImageLoading
{
    // Subject - common interface for real object and its proxy.
    public interface IImage
    {
        string FileName { get; }
        void Display();
    }
}
