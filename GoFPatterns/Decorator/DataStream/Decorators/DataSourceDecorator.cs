namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Basis-Dekorierer – delegiert Aufrufe an die umhüllte Datenquelle.
    public abstract class DataSourceDecorator(IDataSource source) : IDataSource
    {
        protected IDataSource Source { get; } = source;

        public virtual void Write(string data) => Source.Write(data);

        public virtual string Read() => Source.Read();
    }
}
