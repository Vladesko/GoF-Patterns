namespace GoFPatterns.Singleton.Logger
{
    public class DemonstrateLogger
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Singleton: Logger (double-checked locking) ===\n");

            NaiveLogger.Reset();
            var before = NaiveLogger.CreatedInstances;
            Parallel.For(0, 10, _ => GC.KeepAlive(NaiveLogger.Instance));
            Console.WriteLine($"Singleton ingenuo - istanze create da 10 thread: {NaiveLogger.CreatedInstances - before} (dovrebbe essere 1!)");

            Parallel.For(0, 10, i => FileLogger.Instance.Log($"messaggio dal task {i}"));
            Console.WriteLine($"Singleton thread-safe - istanze create: {FileLogger.CreatedInstances}");

            Console.WriteLine("\nTutti i messaggi sono nello stesso logger:");
            foreach (var line in FileLogger.Instance.GetLines().TakeLast(10))
            {
                Console.WriteLine($"  {line}");
            }
        }
    }
}
