namespace GoFPatterns.Command.TextEditor
{
    // Invoker - executes commands and keeps their history for Undo/Redo.
    // It works only with ICommand and doesn't know what commands do.
    public class CommandHistory
    {
        private readonly Stack<ICommand> _undo = new();
        private readonly Stack<ICommand> _redo = new();

        public void Execute(ICommand command)
        {
            command.Execute();
            _undo.Push(command);
            _redo.Clear(); // New action makes redo history invalid.
            Console.WriteLine($"  Do:   {command.Name}");
        }

        public void Undo()
        {
            if (!_undo.TryPop(out var command))
            {
                Console.WriteLine("  Nothing to undo");
                return;
            }
            command.Undo();
            _redo.Push(command);
            Console.WriteLine($"  Undo: {command.Name}");
        }

        public void Redo()
        {
            if (!_redo.TryPop(out var command))
            {
                Console.WriteLine("  Nothing to redo");
                return;
            }
            command.Execute();
            _undo.Push(command);
            Console.WriteLine($"  Redo: {command.Name}");
        }
    }
}
