namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Decorator concreto - non modifica i dati, aggiunge solo il logging attorno alla chiamata.
    public class LoggingDecorator(IDataSource source, string name) : DataSourceDecorator(source)
    {
        public override void Write(string data)
        {
            Console.WriteLine($"  [{name}] scrittura di {data.Length} caratteri");
            base.Write(data);
        }

        public override string Read()
        {
            var data = base.Read();
            Console.WriteLine($"  [{name}] lettura di {data.Length} caratteri");
            return data;
        }
    }
}
