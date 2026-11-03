namespace GoFPatterns.Singleton.Logger
{
    // Anti-example - NOT thread-safe singleton.
    // Two threads can pass "is null" check at the same time and create two instances.
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
            Thread.Sleep(50); // Slow constructor makes race condition visible.
        }

        public static void Reset() => _instance = null;
    }
}
