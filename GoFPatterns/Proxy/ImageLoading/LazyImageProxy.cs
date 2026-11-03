namespace GoFPatterns.Proxy.ImageLoading
{
    // Virtual proxy - postpones creation of heavy object until it is really needed.
    // Client can't see the difference, because proxy implements the same interface.
    public class LazyImageProxy(string fileName) : IImage
    {
        private HighResolutionImage? _realImage;

        public string FileName { get; } = fileName;

        public void Display()
        {
            _realImage ??= new HighResolutionImage(FileName);
            _realImage.Display();
        }
    }
}
