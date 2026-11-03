namespace GoFPatterns.Command.SmartHome.Commands
{
    // Macro comando - esegue una lista di comandi. È anch'esso un comando, quindi può essere associato a un pulsante.
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
