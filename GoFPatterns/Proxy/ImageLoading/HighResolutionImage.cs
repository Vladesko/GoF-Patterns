namespace GoFPatterns.Proxy.ImageLoading
{
    // Реальный субъект - тяжёлый объект. Изображение загружается с диска прямо в конструкторе.
    public class HighResolutionImage : IImage
    {
        public string FileName { get; }

        public HighResolutionImage(string fileName)
        {
            FileName = fileName;
            LoadFromDisk();
        }

        public void Display() => Console.WriteLine($"  Отображение {FileName}");

        private void LoadFromDisk()
        {
            Console.WriteLine($"  Загрузка {FileName} с диска (20 МБ)...");
            Thread.Sleep(200);
        }
    }
}
