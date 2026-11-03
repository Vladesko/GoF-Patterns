namespace GoFPatterns.Proxy.ImageLoading
{
    // Echtes Subjekt – schweres Objekt. Das Bild wird direkt im Konstruktor von der Festplatte geladen.
    public class HighResolutionImage : IImage
    {
        public string FileName { get; }

        public HighResolutionImage(string fileName)
        {
            FileName = fileName;
            LoadFromDisk();
        }

        public void Display() => Console.WriteLine($"  {FileName} wird angezeigt");

        private void LoadFromDisk()
        {
            Console.WriteLine($"  {FileName} wird von der Festplatte geladen (20 MB)...");
            Thread.Sleep(200);
        }
    }
}
