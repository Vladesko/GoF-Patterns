using System.Diagnostics;

namespace GoFPatterns.Proxy.ImageLoading
{
    public class DemonstrateImageLoading
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Proxy: Lazy image loading (virtual proxy) ===\n");

            string[] files = ["sea.jpg", "mountains.jpg", "forest.jpg", "city.jpg", "desert.jpg"];

            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Gallery without proxy - all images are loaded at once:");
            var eagerGallery = files.Select(f => (IImage)new HighResolutionImage(f)).ToList();
            Console.WriteLine($"Gallery opened in {stopwatch.ElapsedMilliseconds} ms\n");

            stopwatch.Restart();
            Console.WriteLine("Gallery with proxy - nothing is loaded:");
            var lazyGallery = files.Select(f => (IImage)new LazyImageProxy(f)).ToList();
            Console.WriteLine($"Gallery opened in {stopwatch.ElapsedMilliseconds} ms\n");

            // User looks only at two images.
            Console.WriteLine("User opens image #2 twice and image #4:");
            lazyGallery[1].Display();
            lazyGallery[1].Display(); // already loaded
            lazyGallery[3].Display();
        }
    }
}
