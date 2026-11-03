namespace GoFPatterns.Decorator.DataStream
{
    // Конкретный компонент - хранит данные "как есть". Эмуляция файла или записи в базе данных.
    public class InMemoryDataSource : IDataSource
    {
        private string _storage = string.Empty;

        public string RawContent => _storage;

        public void Write(string data) => _storage = data;

        public string Read() => _storage;
    }
}
