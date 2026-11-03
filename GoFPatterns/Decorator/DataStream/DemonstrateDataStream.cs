using GoFPatterns.Decorator.DataStream.Decorators;

namespace GoFPatterns.Decorator.DataStream
{
    public class DemonstrateDataStream
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Decorator: Sorgente dati ===\n");

            var data = string.Join(", ", Enumerable.Repeat("cliente: Mario Rossi, carta: 4111-1111-1111-1111", 5));

            // Archivio semplice.
            var plainStorage = new InMemoryDataSource();
            plainStorage.Write(data);
            Console.WriteLine($"Archivio semplice ({plainStorage.RawContent.Length} caratteri):\n  {plainStorage.RawContent[..60]}...\n");

            // Lo stesso archivio, ma avvolto dai decorator a runtime.
            // Scrittura: logging -> compressione -> cifratura -> archivio
            // Lettura:   archivio -> cifratura -> compressione -> logging
            var protectedStorage = new InMemoryDataSource();
            IDataSource source = new LoggingDecorator(
                new CompressionDecorator(
                    new EncryptionDecorator(protectedStorage, "secret")),
                "client");

            source.Write(data);
            Console.WriteLine($"Archivio compresso + cifrato ({protectedStorage.RawContent.Length} caratteri):\n  {protectedStorage.RawContent[..Math.Min(60, protectedStorage.RawContent.Length)]}...");

            var restored = source.Read();
            Console.WriteLine($"I dati sono stati ripristinati correttamente: {restored == data}");
        }
    }
}
