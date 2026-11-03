namespace GoFPatterns.Command.SmartHome
{
    // Invoker - telecomando con pulsanti configurabili. Non sa nulla dei dispositivi.
    // Inoltre può mettere i comandi in coda ed eseguirli in seguito (scheduler).
    public class RemoteControl
    {
        private readonly Dictionary<int, IHomeCommand> _buttons = [];
        private readonly Queue<IHomeCommand> _scheduled = new();

        public void SetCommand(int button, IHomeCommand command) => _buttons[button] = command;

        public void PressButton(int button)
        {
            if (_buttons.TryGetValue(button, out var command))
            {
                Console.WriteLine($"  Pulsante {button}: {command.Description}");
                command.Execute();
            }
            else
            {
                Console.WriteLine($"  Pulsante {button}: non configurato");
            }
        }

        public void Schedule(IHomeCommand command) => _scheduled.Enqueue(command);

        public void RunScheduled()
        {
            while (_scheduled.TryDequeue(out var command))
            {
                Console.WriteLine($"  Pianificato: {command.Description}");
                command.Execute();
            }
        }
    }
}
