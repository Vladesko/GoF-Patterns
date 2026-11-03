namespace GoFPatterns.Decorator.DataStream
{
    // Komponente – Quelle, in die Daten geschrieben und aus der sie gelesen werden können.
    public interface IDataSource
    {
        void Write(string data);
        string Read();
    }
}
