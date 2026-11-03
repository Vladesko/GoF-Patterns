namespace GoFPatterns.Proxy.ImageLoading
{
    // Real subject - oggetto pesante. L'immagine viene caricata dal disco direttamente nel costruttore.
    public class HighResolutionImage : IImage
    {
        public string FileName { get; }

        public HighResolutionImage(string fileName)
        {
            FileName = fileName;
            LoadFromDisk();
        }

        public void Display() => Console.WriteLine($"  Visualizzazione di {FileName}");

        private void LoadFromDisk()
        {
            Console.WriteLine($"  Caricamento di {FileName} dal disco (20 MB)...");
            Thread.Sleep(200);
        }
    }
}
