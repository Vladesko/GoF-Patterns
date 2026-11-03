namespace GoFPatterns.Singleton.Configuration
{
    // Singleton – Anwendungskonfiguration, die nur in einer einzigen Instanz existieren darf.
    // Lazy<T> garantiert von Haus aus eine threadsichere verzögerte Initialisierung.
    // Die Klasse ist sealed, damit niemand von ihr erben und über eine Unterklasse eine zweite Instanz erzeugen kann.
    public sealed class AppConfiguration
    {
        private static readonly Lazy<AppConfiguration> _instance = new(() => new AppConfiguration());

        private readonly Dictionary<string, string> _settings;

        public static AppConfiguration Instance => _instance.Value;

        public Guid InstanceId { get; } = Guid.NewGuid();

        // Privater Konstruktor – außerhalb der Klasse kann niemand "new AppConfiguration()" aufrufen.
        private AppConfiguration()
        {
            // Emulation eines aufwendigen Ladevorgangs, z. B. aus appsettings.json.
            Console.WriteLine("  [AppConfiguration] Einstellungen werden aus der Datei geladen...");
            _settings = new Dictionary<string, string>
            {
                ["ConnectionString"] = "Server=localhost;Database=Shop",
                ["Currency"] = "EUR",
                ["MaxItemsInCart"] = "50"
            };
        }

        public string Get(string key) =>
            _settings.TryGetValue(key, out var value) ? value : throw new KeyNotFoundException(key);

        public void Set(string key, string value) => _settings[key] = value;
    }
}
