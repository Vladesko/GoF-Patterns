using System.Diagnostics;

namespace GoFPatterns.Proxy.ImageLoading
{
    public class DemonstrateImageLoading
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Proxy: carga diferida de imágenes (proxy virtual) ===\n");

            string[] files = ["sea.jpg", "mountains.jpg", "forest.jpg", "city.jpg", "desert.jpg"];

            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Galería sin proxy: todas las imágenes se cargan de golpe:");
            var eagerGallery = files.Select(f => (IImage)new HighResolutionImage(f)).ToList();
            Console.WriteLine($"Galería abierta en {stopwatch.ElapsedMilliseconds} ms\n");

            stopwatch.Restart();
            Console.WriteLine("Galería con proxy: no se carga nada:");
            var lazyGallery = files.Select(f => (IImage)new LazyImageProxy(f)).ToList();
            Console.WriteLine($"Galería abierta en {stopwatch.ElapsedMilliseconds} ms\n");

            // El usuario solo mira dos imágenes.
            Console.WriteLine("El usuario abre la imagen n.º 2 dos veces y la imagen n.º 4:");
            lazyGallery[1].Display();
            lazyGallery[1].Display(); // ya está cargada
            lazyGallery[3].Display();
        }
    }
}
