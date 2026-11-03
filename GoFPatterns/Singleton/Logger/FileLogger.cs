namespace GoFPatterns.Singleton.Logger
{
    // Singleton - implementazione classica con double-checked locking.
    // Mostrata per capire come funziona la thread safety "a mano". Nel codice reale Lazy<T> è più semplice.
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
                // Primo controllo senza lock - percorso veloce quando l'istanza esiste già.
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        // Secondo controllo - un altro thread potrebbe aver creato l'istanza mentre aspettavamo il lock.
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
            Thread.Sleep(50); // Emulazione dell'apertura di un file di log.
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
