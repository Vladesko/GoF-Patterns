namespace GoFPatterns.Command.TextEditor
{
    // Инициатор - выполняет команды и хранит их историю для Undo/Redo.
    // Он работает только с ICommand и не знает, что делают команды.
    public class CommandHistory
    {
        private readonly Stack<ICommand> _undo = new();
        private readonly Stack<ICommand> _redo = new();

        public void Execute(ICommand command)
        {
            command.Execute();
            _undo.Push(command);
            _redo.Clear(); // Новое действие делает историю повтора недействительной.
            Console.WriteLine($"  Выполнить: {command.Name}");
        }

        public void Undo()
        {
            if (!_undo.TryPop(out var command))
            {
                Console.WriteLine("  Нечего отменять");
                return;
            }
            command.Undo();
            _redo.Push(command);
            Console.WriteLine($"  Отменить:  {command.Name}");
        }

        public void Redo()
        {
            if (!_redo.TryPop(out var command))
            {
                Console.WriteLine("  Нечего повторять");
                return;
            }
            command.Execute();
            _undo.Push(command);
            Console.WriteLine($"  Повторить: {command.Name}");
        }
    }
}
