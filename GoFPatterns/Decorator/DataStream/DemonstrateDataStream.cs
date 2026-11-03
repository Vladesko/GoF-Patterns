using GoFPatterns.Decorator.DataStream.Decorators;

namespace GoFPatterns.Decorator.DataStream
{
    public class DemonstrateDataStream
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Decorator: fuente de datos ===\n");

            var data = string.Join(", ", Enumerable.Repeat("cliente: John Smith, tarjeta: 4111-1111-1111-1111", 5));

            // Almacenamiento sin procesar.
            var plainStorage = new InMemoryDataSource();
            plainStorage.Write(data);
            Console.WriteLine($"Almacenamiento sin procesar ({plainStorage.RawContent.Length} caracteres):\n  {plainStorage.RawContent[..60]}...\n");

            // El mismo almacenamiento, pero envuelto con decoradores en tiempo de ejecución.
            // Escritura: logging -> compresión -> cifrado -> almacenamiento
            // Lectura:   almacenamiento -> cifrado -> compresión -> logging
            var protectedStorage = new InMemoryDataSource();
            IDataSource source = new LoggingDecorator(
                new CompressionDecorator(
                    new EncryptionDecorator(protectedStorage, "secret")),
                "cliente");

            source.Write(data);
            Console.WriteLine($"Almacenamiento comprimido + cifrado ({protectedStorage.RawContent.Length} caracteres):\n  {protectedStorage.RawContent[..Math.Min(60, protectedStorage.RawContent.Length)]}...");

            var restored = source.Read();
            Console.WriteLine($"Los datos se restauran correctamente: {restored == data}");
        }
    }
}
