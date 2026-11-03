namespace GoFPatterns.Command.SmartHome
{
    // Invocador: mando con botones configurables. No sabe nada de los dispositivos.
    // Además puede poner comandos en una cola y ejecutarlos más tarde (programador).
    public class RemoteControl
    {
        private readonly Dictionary<int, IHomeCommand> _buttons = [];
        private readonly Queue<IHomeCommand> _scheduled = new();

        public void SetCommand(int button, IHomeCommand command) => _buttons[button] = command;

        public void PressButton(int button)
        {
            if (_buttons.TryGetValue(button, out var command))
            {
                Console.WriteLine($"  Botón {button}: {command.Description}");
                command.Execute();
            }
            else
            {
                Console.WriteLine($"  Botón {button}: no configurado");
            }
        }

        public void Schedule(IHomeCommand command) => _scheduled.Enqueue(command);

        public void RunScheduled()
        {
            while (_scheduled.TryDequeue(out var command))
            {
                Console.WriteLine($"  Programado: {command.Description}");
                command.Execute();
            }
        }
    }
}
