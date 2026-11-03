namespace GoFPatterns.Singleton.Logger
{
    public class DemonstrateLogger
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Singleton: Logger (Double-Checked Locking) ===\n");

            NaiveLogger.Reset();
            var before = NaiveLogger.CreatedInstances;
            Parallel.For(0, 10, _ => GC.KeepAlive(NaiveLogger.Instance));
            Console.WriteLine($"Naives Singleton – von 10 Threads erzeugte Instanzen: {NaiveLogger.CreatedInstances - before} (sollte 1 sein!)");

            Parallel.For(0, 10, i => FileLogger.Instance.Log($"Nachricht von Task {i}"));
            Console.WriteLine($"Threadsicheres Singleton – erzeugte Instanzen: {FileLogger.CreatedInstances}");

            Console.WriteLine("\nAlle Nachrichten landen im selben Logger:");
            foreach (var line in FileLogger.Instance.GetLines().TakeLast(10))
            {
                Console.WriteLine($"  {line}");
            }
        }
    }
}
