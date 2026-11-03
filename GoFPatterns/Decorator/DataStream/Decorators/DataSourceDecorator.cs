namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Base decorator - delegates calls to wrapped data source.
    public abstract class DataSourceDecorator(IDataSource source) : IDataSource
    {
        protected IDataSource Source { get; } = source;

        public virtual void Write(string data) => Source.Write(data);

        public virtual string Read() => Source.Read();
    }
}
