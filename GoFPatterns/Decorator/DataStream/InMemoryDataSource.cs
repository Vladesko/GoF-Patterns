namespace GoFPatterns.Decorator.DataStream
{
    // Component concreto - memorizza i dati "così come sono". Emulazione di un file o di un record di database.
    public class InMemoryDataSource : IDataSource
    {
        private string _storage = string.Empty;

        public string RawContent => _storage;

        public void Write(string data) => _storage = data;

        public string Read() => _storage;
    }
}
