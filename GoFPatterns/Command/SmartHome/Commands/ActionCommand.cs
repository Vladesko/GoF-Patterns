namespace GoFPatterns.Command.SmartHome.Commands
{
    // Конкретная команда на основе делегата. В C# простая команда может быть обычной лямбдой,
    // поэтому не нужен отдельный класс для каждого действия каждого устройства.
    public class ActionCommand(string description, Action action) : IHomeCommand
    {
        public string Description { get; } = description;

        public void Execute() => action();
    }
}
