namespace GoFPatterns.Proxy.ImageLoading
{
    // Virtual proxy - rimanda la creazione dell'oggetto pesante finché non serve davvero.
    // Il client non vede la differenza, perché il proxy implementa la stessa interfaccia.
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
