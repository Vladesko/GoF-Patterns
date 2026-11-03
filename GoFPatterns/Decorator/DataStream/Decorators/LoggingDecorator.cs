namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Konkreter Dekorierer – ändert die Daten nicht, sondern ergänzt nur Logging rund um den Aufruf.
    public class LoggingDecorator(IDataSource source, string name) : DataSourceDecorator(source)
    {
        public override void Write(string data)
        {
            Console.WriteLine($"  [{name}] Schreiben: {data.Length} Zeichen");
            base.Write(data);
        }

        public override string Read()
        {
            var data = base.Read();
            Console.WriteLine($"  [{name}] Lesen: {data.Length} Zeichen");
            return data;
        }
    }
}
