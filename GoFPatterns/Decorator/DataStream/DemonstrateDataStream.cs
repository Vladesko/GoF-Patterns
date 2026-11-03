using GoFPatterns.Decorator.DataStream.Decorators;

namespace GoFPatterns.Decorator.DataStream
{
    public class DemonstrateDataStream
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Декоратор: источник данных ===\n");

            var data = string.Join(", ", Enumerable.Repeat("клиент: Иван Петров, карта: 4111-1111-1111-1111", 5));

            // Обычное хранилище.
            var plainStorage = new InMemoryDataSource();
            plainStorage.Write(data);
            Console.WriteLine($"Обычное хранилище ({plainStorage.RawContent.Length} символов):\n  {plainStorage.RawContent[..60]}...\n");

            // То же хранилище, но обёрнутое декораторами во время выполнения.
            // Запись: логирование -> сжатие -> шифрование -> хранилище
            // Чтение: хранилище -> шифрование -> сжатие -> логирование
            var protectedStorage = new InMemoryDataSource();
            IDataSource source = new LoggingDecorator(
                new CompressionDecorator(
                    new EncryptionDecorator(protectedStorage, "secret")),
                "клиент");

            source.Write(data);
            Console.WriteLine($"Сжатое + зашифрованное хранилище ({protectedStorage.RawContent.Length} символов):\n  {protectedStorage.RawContent[..Math.Min(60, protectedStorage.RawContent.Length)]}...");

            var restored = source.Read();
            Console.WriteLine($"Данные восстановлены корректно: {restored == data}");
        }
    }
}
