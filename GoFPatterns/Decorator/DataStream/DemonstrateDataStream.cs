using GoFPatterns.Decorator.DataStream.Decorators;

namespace GoFPatterns.Decorator.DataStream
{
    public class DemonstrateDataStream
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Decorator: Data source ===\n");

            var data = string.Join(", ", Enumerable.Repeat("customer: John Smith, card: 4111-1111-1111-1111", 5));

            // Plain storage.
            var plainStorage = new InMemoryDataSource();
            plainStorage.Write(data);
            Console.WriteLine($"Plain storage ({plainStorage.RawContent.Length} chars):\n  {plainStorage.RawContent[..60]}...\n");

            // The same storage, but wrapped with decorators at runtime.
            // Write: logging -> compression -> encryption -> storage
            // Read:  storage -> encryption -> compression -> logging
            var protectedStorage = new InMemoryDataSource();
            IDataSource source = new LoggingDecorator(
                new CompressionDecorator(
                    new EncryptionDecorator(protectedStorage, "secret")),
                "client");

            source.Write(data);
            Console.WriteLine($"Compressed + encrypted storage ({protectedStorage.RawContent.Length} chars):\n  {protectedStorage.RawContent[..Math.Min(60, protectedStorage.RawContent.Length)]}...");

            var restored = source.Read();
            Console.WriteLine($"Data is restored correctly: {restored == data}");
        }
    }
}
