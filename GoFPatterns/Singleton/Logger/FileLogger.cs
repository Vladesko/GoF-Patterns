namespace GoFPatterns.Singleton.Logger
{
    // Singleton: implementación clásica con double-checked locking (doble comprobación con bloqueo).
    // Se muestra para entender cómo funciona la seguridad para hilos "a mano". En código real Lazy<T> es más sencillo.
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
                // Primera comprobación sin bloqueo: camino rápido cuando la instancia ya existe.
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        // Segunda comprobación: otro hilo pudo crear la instancia mientras esperábamos el bloqueo.
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
            Thread.Sleep(50); // Emulación de la apertura de un archivo de log.
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
