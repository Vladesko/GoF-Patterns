namespace GoFPatterns.Singleton.Logger
{
    // Одиночка - классическая реализация с блокировкой с двойной проверкой (double-checked locking).
    // Показана, чтобы понять, как потокобезопасность работает "вручную". В реальном коде проще Lazy<T>.
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
                // Первая проверка без блокировки - быстрый путь, когда экземпляр уже существует.
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        // Вторая проверка - другой поток мог создать экземпляр, пока мы ждали блокировку.
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
            Thread.Sleep(50); // Эмуляция открытия файла журнала.
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
