namespace GoFPatterns.Proxy.ImageLoading
{
    // Real subject - heavy object. Image is loaded from disk right in the constructor.
    public class HighResolutionImage : IImage
    {
        public string FileName { get; }

        public HighResolutionImage(string fileName)
        {
            FileName = fileName;
            LoadFromDisk();
        }

        public void Display() => Console.WriteLine($"  Displaying {FileName}");

        private void LoadFromDisk()
        {
            Console.WriteLine($"  Loading {FileName} from disk (20 MB)...");
            Thread.Sleep(200);
        }
    }
}
