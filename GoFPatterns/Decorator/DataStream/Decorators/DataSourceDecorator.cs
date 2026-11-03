namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Decorator base - delega le chiamate alla sorgente dati avvolta.
    public abstract class DataSourceDecorator(IDataSource source) : IDataSource
    {
        protected IDataSource Source { get; } = source;

        public virtual void Write(string data) => Source.Write(data);

        public virtual string Read() => Source.Read();
    }
}
