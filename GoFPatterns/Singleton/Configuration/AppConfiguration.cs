namespace GoFPatterns.Singleton.Configuration
{
    // Singleton - configurazione dell'applicazione che deve esistere in un'unica istanza.
    // Lazy<T> garantisce di serie un'inizializzazione lazy thread-safe.
    // La classe è sealed, così nessuno può ereditarla e creare una seconda istanza tramite una sottoclasse.
    public sealed class AppConfiguration
    {
        private static readonly Lazy<AppConfiguration> _instance = new(() => new AppConfiguration());

        private readonly Dictionary<string, string> _settings;

        public static AppConfiguration Instance => _instance.Value;

        public Guid InstanceId { get; } = Guid.NewGuid();

        // Costruttore privato - nessuno può chiamare "new AppConfiguration()" fuori dalla classe.
        private AppConfiguration()
        {
            // Emulazione di un caricamento costoso, ad esempio da appsettings.json.
            Console.WriteLine("  [AppConfiguration] Caricamento delle impostazioni dal file...");
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
