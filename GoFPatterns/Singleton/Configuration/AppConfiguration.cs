namespace GoFPatterns.Singleton.Configuration
{
    // Singleton - application configuration which must exist in a single instance.
    // Lazy<T> guarantees thread-safe lazy initialization out of the box.
    // Class is sealed, so nobody can inherit it and create a second instance through a subclass.
    public sealed class AppConfiguration
    {
        private static readonly Lazy<AppConfiguration> _instance = new(() => new AppConfiguration());

        private readonly Dictionary<string, string> _settings;

        public static AppConfiguration Instance => _instance.Value;

        public Guid InstanceId { get; } = Guid.NewGuid();

        // Private constructor - nobody can call "new AppConfiguration()" outside the class.
        private AppConfiguration()
        {
            // Emulation of expensive loading, for example from appsettings.json.
            Console.WriteLine("  [AppConfiguration] Loading settings from file...");
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
