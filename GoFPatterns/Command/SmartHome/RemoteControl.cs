namespace GoFPatterns.Command.SmartHome
{
    // Инициатор - пульт с настраиваемыми кнопками. Он ничего не знает об устройствах.
    // Также он может ставить команды в очередь и выполнять их позже (планировщик).
    public class RemoteControl
    {
        private readonly Dictionary<int, IHomeCommand> _buttons = [];
        private readonly Queue<IHomeCommand> _scheduled = new();

        public void SetCommand(int button, IHomeCommand command) => _buttons[button] = command;

        public void PressButton(int button)
        {
            if (_buttons.TryGetValue(button, out var command))
            {
                Console.WriteLine($"  Кнопка {button}: {command.Description}");
                command.Execute();
            }
            else
            {
                Console.WriteLine($"  Кнопка {button}: не настроена");
            }
        }

        public void Schedule(IHomeCommand command) => _scheduled.Enqueue(command);

        public void RunScheduled()
        {
            while (_scheduled.TryDequeue(out var command))
            {
                Console.WriteLine($"  Запланировано: {command.Description}");
                command.Execute();
            }
        }
    }
}
