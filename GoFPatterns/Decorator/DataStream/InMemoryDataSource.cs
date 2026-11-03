namespace GoFPatterns.Decorator.DataStream
{
    // Concrete component - stores data "as is". Emulation of a file or a database record.
    public class InMemoryDataSource : IDataSource
    {
        private string _storage = string.Empty;

        public string RawContent => _storage;

        public void Write(string data) => _storage = data;

        public string Read() => _storage;
    }
}
