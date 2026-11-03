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
            Console.WriteLine($"Naive singleton - instances created by 10 threads: {NaiveLogger.CreatedInstances - before} (should be 1!)");

            Parallel.For(0, 10, i => FileLogger.Instance.Log($"message from task {i}"));
            Console.WriteLine($"Thread-safe singleton - instances created: {FileLogger.CreatedInstances}");

            Console.WriteLine("\nAll messages are in the same logger:");
            foreach (var line in FileLogger.Instance.GetLines().TakeLast(10))
            {
                Console.WriteLine($"  {line}");
            }
        }
    }
}
