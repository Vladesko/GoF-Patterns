namespace GoFPatterns.Singleton.Logger
{
    public class DemonstrateLogger
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Singleton: logger (double-checked locking) ===\n");

            NaiveLogger.Reset();
            var before = NaiveLogger.CreatedInstances;
            Parallel.For(0, 10, _ => GC.KeepAlive(NaiveLogger.Instance));
            Console.WriteLine($"Singleton ingenuo: instancias creadas por 10 hilos: {NaiveLogger.CreatedInstances - before} (¡debería ser 1!)");

            Parallel.For(0, 10, i => FileLogger.Instance.Log($"mensaje de la tarea {i}"));
            Console.WriteLine($"Singleton seguro para hilos: instancias creadas: {FileLogger.CreatedInstances}");

            Console.WriteLine("\nTodos los mensajes están en el mismo logger:");
            foreach (var line in FileLogger.Instance.GetLines().TakeLast(10))
            {
                Console.WriteLine($"  {line}");
            }
        }
    }
}
