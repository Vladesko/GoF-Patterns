using System.Diagnostics;

namespace GoFPatterns.Proxy.ImageLoading
{
    public class DemonstrateImageLoading
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Proxy: Verzögertes Laden von Bildern (virtueller Proxy) ===\n");

            string[] files = ["sea.jpg", "mountains.jpg", "forest.jpg", "city.jpg", "desert.jpg"];

            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Galerie ohne Proxy – alle Bilder werden sofort geladen:");
            var eagerGallery = files.Select(f => (IImage)new HighResolutionImage(f)).ToList();
            Console.WriteLine($"Galerie in {stopwatch.ElapsedMilliseconds} ms geöffnet\n");

            stopwatch.Restart();
            Console.WriteLine("Galerie mit Proxy – nichts wird geladen:");
            var lazyGallery = files.Select(f => (IImage)new LazyImageProxy(f)).ToList();
            Console.WriteLine($"Galerie in {stopwatch.ElapsedMilliseconds} ms geöffnet\n");

            // Der Benutzer sieht sich nur zwei Bilder an.
            Console.WriteLine("Der Benutzer öffnet Bild Nr. 2 zweimal und Bild Nr. 4:");
            lazyGallery[1].Display();
            lazyGallery[1].Display(); // bereits geladen
            lazyGallery[3].Display();
        }
    }
}
