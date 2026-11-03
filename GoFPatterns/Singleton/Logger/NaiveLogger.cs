namespace GoFPatterns.Singleton.Logger
{
    // Esempio negativo - singleton NON thread-safe.
    // Due thread possono superare il controllo "is null" contemporaneamente e creare due istanze.
    public sealed class NaiveLogger
    {
        private static NaiveLogger? _instance;

        private static int _createdInstances;
        public static int CreatedInstances => _createdInstances;

        public static NaiveLogger Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new NaiveLogger();
                }
                return _instance;
            }
        }

        private NaiveLogger()
        {
            Interlocked.Increment(ref _createdInstances);
            Thread.Sleep(50); // Il costruttore lento rende visibile la race condition.
        }

        public static void Reset() => _instance = null;
    }
}
