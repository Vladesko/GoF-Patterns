namespace GoFPatterns.Singleton.Logger
{
    // Singleton – klassische Implementierung mit Double-Checked Locking.
    // Gezeigt, um zu verstehen, wie Threadsicherheit "von Hand" funktioniert. In echtem Code ist Lazy<T> einfacher.
    public sealed class FileLogger
    {
        private static readonly object _lock = new();
        private static volatile FileLogger? _instance;

        private readonly List<string> _lines = [];

        public static int CreatedInstances { get; private set; }

        public static FileLogger Instance
        {
            get
            {
                // Erste Prüfung ohne Sperre – schneller Weg, wenn die Instanz bereits existiert.
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        // Zweite Prüfung – ein anderer Thread könnte die Instanz erzeugt haben, während wir auf die Sperre gewartet haben.
                        if (_instance is null)
                        {
                            _instance = new FileLogger();
                        }
                    }
                }
                return _instance;
            }
        }

        private FileLogger()
        {
            CreatedInstances++;
            Thread.Sleep(50); // Emulation des Öffnens einer Logdatei.
        }

        public void Log(string message)
        {
            lock (_lines)
            {
                _lines.Add($"{DateTime.Now:HH:mm:ss.fff} {message}");
            }
        }

        public IReadOnlyList<string> GetLines()
        {
            lock (_lines)
            {
                return [.. _lines];
            }
        }
    }
}
