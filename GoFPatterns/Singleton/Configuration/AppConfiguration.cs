namespace GoFPatterns.Singleton.Configuration
{
    // Одиночка - конфигурация приложения, которая должна существовать в единственном экземпляре.
    // Lazy<T> из коробки гарантирует потокобезопасную ленивую инициализацию.
    // Класс sealed, поэтому никто не сможет унаследоваться от него и создать второй экземпляр через подкласс.
    public sealed class AppConfiguration
    {
        private static readonly Lazy<AppConfiguration> _instance = new(() => new AppConfiguration());

        private readonly Dictionary<string, string> _settings;

        public static AppConfiguration Instance => _instance.Value;

        public Guid InstanceId { get; } = Guid.NewGuid();

        // Приватный конструктор - никто не может вызвать "new AppConfiguration()" вне класса.
        private AppConfiguration()
        {
            // Эмуляция дорогой загрузки, например из appsettings.json.
            Console.WriteLine("  [AppConfiguration] Загрузка настроек из файла...");
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
