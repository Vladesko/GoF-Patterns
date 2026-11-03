namespace GoFPatterns.Command.SmartHome.Commands
{
    // Konkreter Befehl auf Basis eines Delegaten. In C# kann ein einfacher Befehl einfach ein Lambda sein,
    // daher braucht man keine eigene Klasse für jede Aktion jedes Geräts.
    public class ActionCommand(string description, Action action) : IHomeCommand
    {
        public string Description { get; } = description;

        public void Execute() => action();
    }
}
