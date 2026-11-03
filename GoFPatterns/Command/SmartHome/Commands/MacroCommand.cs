namespace GoFPatterns.Command.SmartHome.Commands
{
    // Macrocomando: ejecuta una lista de comandos. También es un comando, así que se puede asignar a un botón.
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
