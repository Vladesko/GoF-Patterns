namespace GoFPatterns.Singleton.Logger
{
    // Antiejemplo: singleton NO seguro para hilos.
    // Dos hilos pueden pasar la comprobación "is null" al mismo tiempo y crear dos instancias.
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
            Thread.Sleep(50); // Un constructor lento hace visible la condición de carrera.
        }

        public static void Reset() => _instance = null;
    }
}
