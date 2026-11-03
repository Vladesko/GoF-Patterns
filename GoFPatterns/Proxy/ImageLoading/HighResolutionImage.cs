namespace GoFPatterns.Proxy.ImageLoading
{
    // Sujeto real: objeto pesado. La imagen se carga desde el disco directamente en el constructor.
    public class HighResolutionImage : IImage
    {
        public string FileName { get; }

        public HighResolutionImage(string fileName)
        {
            FileName = fileName;
            LoadFromDisk();
        }

        public void Display() => Console.WriteLine($"  Mostrando {FileName}");

        private void LoadFromDisk()
        {
            Console.WriteLine($"  Cargando {FileName} desde el disco (20 MB)...");
            Thread.Sleep(200);
        }
    }
}
