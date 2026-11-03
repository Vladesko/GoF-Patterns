namespace GoFPatterns.Proxy.ImageLoading
{
    // Proxy virtual: pospone la creación del objeto pesado hasta que realmente se necesita.
    // El cliente no puede ver la diferencia, porque el proxy implementa la misma interfaz.
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
