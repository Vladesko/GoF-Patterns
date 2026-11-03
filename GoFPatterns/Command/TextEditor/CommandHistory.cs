namespace GoFPatterns.Command.TextEditor
{
    // Invoker - esegue i comandi e ne conserva la cronologia per Undo/Redo.
    // Lavora solo con ICommand e non sa cosa fanno i comandi.
    public class CommandHistory
    {
        private readonly Stack<ICommand> _undo = new();
        private readonly Stack<ICommand> _redo = new();

        public void Execute(ICommand command)
        {
            command.Execute();
            _undo.Push(command);
            _redo.Clear(); // Una nuova azione invalida la cronologia di redo.
            Console.WriteLine($"  Esegui:  {command.Name}");
        }

        public void Undo()
        {
            if (!_undo.TryPop(out var command))
            {
                Console.WriteLine("  Niente da annullare");
                return;
            }
            command.Undo();
            _redo.Push(command);
            Console.WriteLine($"  Annulla: {command.Name}");
        }

        public void Redo()
        {
            if (!_redo.TryPop(out var command))
            {
                Console.WriteLine("  Niente da ripristinare");
                return;
            }
            command.Execute();
            _undo.Push(command);
            Console.WriteLine($"  Ripeti:  {command.Name}");
        }
    }
}
