namespace GoFPatterns.Decorator.DataStream
{
    // Componente: fuente en la que se pueden escribir y de la que se pueden leer datos.
    public interface IDataSource
    {
        void Write(string data);
        string Read();
    }
}
