namespace GoFPatterns.Singleton.Logger
{
    public class DemonstrateLogger
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Одиночка: логгер (блокировка с двойной проверкой) ===\n");

            NaiveLogger.Reset();
            var before = NaiveLogger.CreatedInstances;
            Parallel.For(0, 10, _ => GC.KeepAlive(NaiveLogger.Instance));
            Console.WriteLine($"Наивный одиночка - экземпляров создано 10 потоками: {NaiveLogger.CreatedInstances - before} (должен быть 1!)");

            Parallel.For(0, 10, i => FileLogger.Instance.Log($"сообщение из задачи {i}"));
            Console.WriteLine($"Потокобезопасный одиночка - экземпляров создано: {FileLogger.CreatedInstances}");

            Console.WriteLine("\nВсе сообщения находятся в одном логгере:");
            foreach (var line in FileLogger.Instance.GetLines().TakeLast(10))
            {
                Console.WriteLine($"  {line}");
            }
        }
    }
}
