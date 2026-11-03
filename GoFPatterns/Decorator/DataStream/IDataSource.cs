namespace GoFPatterns.Decorator.DataStream
{
    // Component - sorgente in cui i dati possono essere scritti e da cui possono essere letti.
    public interface IDataSource
    {
        void Write(string data);
        string Read();
    }
}
