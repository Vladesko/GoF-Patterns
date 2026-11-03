using GoFPatterns.Decorator.DataStream.Decorators;

namespace GoFPatterns.Decorator.DataStream
{
    public class DemonstrateDataStream
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Decorator: Datenquelle ===\n");

            var data = string.Join(", ", Enumerable.Repeat("Kunde: Max Mustermann, Karte: 4111-1111-1111-1111", 5));

            // Einfacher Speicher.
            var plainStorage = new InMemoryDataSource();
            plainStorage.Write(data);
            Console.WriteLine($"Einfacher Speicher ({plainStorage.RawContent.Length} Zeichen):\n  {plainStorage.RawContent[..60]}...\n");

            // Derselbe Speicher, aber zur Laufzeit mit Dekorierern umhüllt.
            // Schreiben: Logging -> Komprimierung -> Verschlüsselung -> Speicher
            // Lesen:     Speicher -> Verschlüsselung -> Komprimierung -> Logging
            var protectedStorage = new InMemoryDataSource();
            IDataSource source = new LoggingDecorator(
                new CompressionDecorator(
                    new EncryptionDecorator(protectedStorage, "secret")),
                "Client");

            source.Write(data);
            Console.WriteLine($"Komprimierter + verschlüsselter Speicher ({protectedStorage.RawContent.Length} Zeichen):\n  {protectedStorage.RawContent[..Math.Min(60, protectedStorage.RawContent.Length)]}...");

            var restored = source.Read();
            Console.WriteLine($"Daten korrekt wiederhergestellt: {restored == data}");
        }
    }
}
