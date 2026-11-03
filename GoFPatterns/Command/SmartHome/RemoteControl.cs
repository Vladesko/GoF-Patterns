namespace GoFPatterns.Command.SmartHome
{
    // Invoker - remote with configurable buttons. It knows nothing about devices.
    // Also it can put commands into a queue and execute them later (scheduler).
    public class RemoteControl
    {
        private readonly Dictionary<int, IHomeCommand> _buttons = [];
        private readonly Queue<IHomeCommand> _scheduled = new();

        public void SetCommand(int button, IHomeCommand command) => _buttons[button] = command;

        public void PressButton(int button)
        {
            if (_buttons.TryGetValue(button, out var command))
            {
                Console.WriteLine($"  Button {button}: {command.Description}");
                command.Execute();
            }
            else
            {
                Console.WriteLine($"  Button {button}: not configured");
            }
        }

        public void Schedule(IHomeCommand command) => _scheduled.Enqueue(command);

        public void RunScheduled()
        {
            while (_scheduled.TryDequeue(out var command))
            {
                Console.WriteLine($"  Scheduled: {command.Description}");
                command.Execute();
            }
        }
    }
}
