namespace GoFPatterns.Command.SmartHome.Commands
{
    // Command concreto basato su un delegate. In C# un comando semplice può essere solo una lambda,
    // quindi non serve una classe separata per ogni azione di ogni dispositivo.
    public class ActionCommand(string description, Action action) : IHomeCommand
    {
        public string Description { get; } = description;

        public void Execute() => action();
    }
}
