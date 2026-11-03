namespace GoFPatterns.Command.SmartHome.Commands
{
    // Макрокоманда - выполняет список команд. Она тоже команда, поэтому её можно назначить на кнопку.
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
