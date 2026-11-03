namespace GoFPatterns.Singleton.Logger
{
    // Антипример - НЕ потокобезопасный одиночка.
    // Два потока могут одновременно пройти проверку "is null" и создать два экземпляра.
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
            Thread.Sleep(50); // Медленный конструктор делает состояние гонки заметным.
        }

        public static void Reset() => _instance = null;
    }
}
