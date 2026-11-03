namespace GoFPatterns.Decorator.DataStream
{
    // Component - source where data can be written and read from.
    public interface IDataSource
    {
        void Write(string data);
        string Read();
    }
}
