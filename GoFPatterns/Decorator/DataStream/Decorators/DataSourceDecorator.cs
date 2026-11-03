namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Decorador base: delega las llamadas en la fuente de datos envuelta.
    public abstract class DataSourceDecorator(IDataSource source) : IDataSource
    {
        protected IDataSource Source { get; } = source;

        public virtual void Write(string data) => Source.Write(data);

        public virtual string Read() => Source.Read();
    }
}
