using System.Diagnostics;

namespace GoFPatterns.Proxy.ImageLoading
{
    public class DemonstrateImageLoading
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Proxy: caricamento lazy delle immagini (virtual proxy) ===\n");

            string[] files = ["mare.jpg", "montagne.jpg", "foresta.jpg", "citta.jpg", "deserto.jpg"];

            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Galleria senza proxy - tutte le immagini vengono caricate subito:");
            var eagerGallery = files.Select(f => (IImage)new HighResolutionImage(f)).ToList();
            Console.WriteLine($"Galleria aperta in {stopwatch.ElapsedMilliseconds} ms\n");

            stopwatch.Restart();
            Console.WriteLine("Galleria con proxy - non viene caricato nulla:");
            var lazyGallery = files.Select(f => (IImage)new LazyImageProxy(f)).ToList();
            Console.WriteLine($"Galleria aperta in {stopwatch.ElapsedMilliseconds} ms\n");

            // L'utente guarda solo due immagini.
            Console.WriteLine("L'utente apre due volte l'immagine #2 e l'immagine #4:");
            lazyGallery[1].Display();
            lazyGallery[1].Display(); // già caricata
            lazyGallery[3].Display();
        }
    }
}
