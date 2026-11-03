namespace GoFPatterns.Singleton.Logger
{
    // Negativbeispiel – NICHT threadsicheres Singleton.
    // Zwei Threads können die Prüfung "is null" gleichzeitig passieren und zwei Instanzen erzeugen.
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
            Thread.Sleep(50); // Der langsame Konstruktor macht die Race Condition sichtbar.
        }

        public static void Reset() => _instance = null;
    }
}
