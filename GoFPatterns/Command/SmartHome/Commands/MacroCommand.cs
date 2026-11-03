namespace GoFPatterns.Command.SmartHome.Commands
{
    // Makrobefehl – führt eine Liste von Befehlen aus. Er ist selbst ein Befehl und kann daher einer Taste zugewiesen werden.
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
