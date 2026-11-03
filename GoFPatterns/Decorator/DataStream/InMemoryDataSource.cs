namespace GoFPatterns.Decorator.DataStream
{
    // Konkrete Komponente – speichert Daten "wie sie sind". Emulation einer Datei oder eines Datenbankeintrags.
    public class InMemoryDataSource : IDataSource
    {
        private string _storage = string.Empty;

        public string RawContent => _storage;

        public void Write(string data) => _storage = data;

        public string Read() => _storage;
    }
}
