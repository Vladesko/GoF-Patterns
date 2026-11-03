using System.Diagnostics;

namespace GoFPatterns.Proxy.ImageLoading
{
    public class DemonstrateImageLoading
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Заместитель: ленивая загрузка изображений (виртуальный заместитель) ===\n");

            string[] files = ["sea.jpg", "mountains.jpg", "forest.jpg", "city.jpg", "desert.jpg"];

            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Галерея без заместителя - все изображения загружаются сразу:");
            var eagerGallery = files.Select(f => (IImage)new HighResolutionImage(f)).ToList();
            Console.WriteLine($"Галерея открыта за {stopwatch.ElapsedMilliseconds} мс\n");

            stopwatch.Restart();
            Console.WriteLine("Галерея с заместителем - ничего не загружается:");
            var lazyGallery = files.Select(f => (IImage)new LazyImageProxy(f)).ToList();
            Console.WriteLine($"Галерея открыта за {stopwatch.ElapsedMilliseconds} мс\n");

            // Пользователь смотрит только два изображения.
            Console.WriteLine("Пользователь открывает изображение №2 дважды и изображение №4:");
            lazyGallery[1].Display();
            lazyGallery[1].Display(); // уже загружено
            lazyGallery[3].Display();
        }
    }
}
