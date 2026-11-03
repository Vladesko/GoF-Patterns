namespace GoFPatterns.Command.SmartHome.Commands
{
    // Concrete command based on delegate. In C# a simple command can be just a lambda,
    // so we don't need a separate class for each action of each device.
    public class ActionCommand(string description, Action action) : IHomeCommand
    {
        public string Description { get; } = description;

        public void Execute() => action();
    }
}
