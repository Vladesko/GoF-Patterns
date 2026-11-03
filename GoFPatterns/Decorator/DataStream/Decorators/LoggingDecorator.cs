namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Concrete decorator - doesn't change data, only adds logging around the call.
    public class LoggingDecorator(IDataSource source, string name) : DataSourceDecorator(source)
    {
        public override void Write(string data)
        {
            Console.WriteLine($"  [{name}] write {data.Length} chars");
            base.Write(data);
        }

        public override string Read()
        {
            var data = base.Read();
            Console.WriteLine($"  [{name}] read {data.Length} chars");
            return data;
        }
    }
}
