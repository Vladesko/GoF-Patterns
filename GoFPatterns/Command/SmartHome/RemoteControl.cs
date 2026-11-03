namespace GoFPatterns.Command.SmartHome
{
    // Aufrufer (Invoker) – Fernbedienung mit konfigurierbaren Tasten. Sie weiß nichts über die Geräte.
    // Außerdem kann sie Befehle in eine Warteschlange stellen und später ausführen (Scheduler).
    public class RemoteControl
    {
        private readonly Dictionary<int, IHomeCommand> _buttons = [];
        private readonly Queue<IHomeCommand> _scheduled = new();

        public void SetCommand(int button, IHomeCommand command) => _buttons[button] = command;

        public void PressButton(int button)
        {
            if (_buttons.TryGetValue(button, out var command))
            {
                Console.WriteLine($"  Taste {button}: {command.Description}");
                command.Execute();
            }
            else
            {
                Console.WriteLine($"  Taste {button}: nicht belegt");
            }
        }

        public void Schedule(IHomeCommand command) => _scheduled.Enqueue(command);

        public void RunScheduled()
        {
            while (_scheduled.TryDequeue(out var command))
            {
                Console.WriteLine($"  Geplant: {command.Description}");
                command.Execute();
            }
        }
    }
}
