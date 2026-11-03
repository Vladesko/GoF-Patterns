namespace GoFPatterns.Singleton.Configuration
{
    // Singleton: configuración de la aplicación que debe existir en una única instancia.
    // Lazy<T> garantiza de serie una inicialización diferida segura para hilos.
    // La clase es sealed, así que nadie puede heredarla y crear una segunda instancia mediante una subclase.
    public sealed class AppConfiguration
    {
        private static readonly Lazy<AppConfiguration> _instance = new(() => new AppConfiguration());

        private readonly Dictionary<string, string> _settings;

        public static AppConfiguration Instance => _instance.Value;

        public Guid InstanceId { get; } = Guid.NewGuid();

        // Constructor privado: nadie puede llamar a "new AppConfiguration()" fuera de la clase.
        private AppConfiguration()
        {
            // Emulación de una carga costosa, por ejemplo desde appsettings.json.
            Console.WriteLine("  [AppConfiguration] Cargando la configuración desde el archivo...");
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
