namespace GoFPatterns.Decorator.DataStream
{
    // Componente concreto: guarda los datos "tal cual". Emulación de un archivo o de un registro de base de datos.
    public class InMemoryDataSource : IDataSource
    {
        private string _storage = string.Empty;

        public string RawContent => _storage;

        public void Write(string data) => _storage = data;

        public string Read() => _storage;
    }
}
