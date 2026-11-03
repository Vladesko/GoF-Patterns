namespace GoFPatterns.Decorator.DataStream
{
    // Компонент - источник, в который можно записывать данные и из которого можно их читать.
    public interface IDataSource
    {
        void Write(string data);
        string Read();
    }
}
