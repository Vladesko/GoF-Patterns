namespace GoFPatterns.Proxy.ImageLoading
{
    // Виртуальный заместитель - откладывает создание тяжёлого объекта до момента, когда он действительно нужен.
    // Клиент не видит разницы, потому что заместитель реализует тот же интерфейс.
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
