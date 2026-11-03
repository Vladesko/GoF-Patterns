namespace GoFPatterns.Proxy.ImageLoading
{
    // Субъект - общий интерфейс для реального объекта и его заместителя.
    public interface IImage
    {
        string FileName { get; }
        void Display();
    }
}
