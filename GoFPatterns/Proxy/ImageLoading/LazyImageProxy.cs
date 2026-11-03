namespace GoFPatterns.Proxy.ImageLoading
{
    // Virtueller Proxy – verschiebt die Erzeugung des schweren Objekts, bis es wirklich gebraucht wird.
    // Der Client merkt keinen Unterschied, weil der Proxy dieselbe Schnittstelle implementiert.
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
