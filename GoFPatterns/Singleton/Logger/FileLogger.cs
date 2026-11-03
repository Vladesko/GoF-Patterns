namespace GoFPatterns.Singleton.Logger
{
    // Singleton - classic implementation with double-checked locking.
    // Shown to understand how thread safety works "by hand". In real code Lazy<T> is simpler.
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
                // First check without lock - fast path when instance already exists.
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        // Second check - other thread could create instance while we were waiting for lock.
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
            Thread.Sleep(50); // Emulation of opening a log file.
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
