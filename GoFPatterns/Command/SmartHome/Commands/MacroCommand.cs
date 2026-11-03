namespace GoFPatterns.Command.SmartHome.Commands
{
    // Macro command - executes a list of commands. It is a command too, so it can be bound to a button.
    public class MacroCommand(string description, params IHomeCommand[] commands) : IHomeCommand
    {
        public string Description { get; } = description;

        public void Execute()
        {
            foreach (var command in commands)
            {
                command.Execute();
            }
        }
    }
}
