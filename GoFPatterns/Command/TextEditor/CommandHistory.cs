namespace GoFPatterns.Command.TextEditor
{
    // Aufrufer (Invoker) – führt Befehle aus und speichert ihren Verlauf für Undo/Redo.
    // Er arbeitet nur mit ICommand und weiß nicht, was die Befehle tun.
    public class CommandHistory
    {
        private readonly Stack<ICommand> _undo = new();
        private readonly Stack<ICommand> _redo = new();

        public void Execute(ICommand command)
        {
            command.Execute();
            _undo.Push(command);
            _redo.Clear(); // Eine neue Aktion macht den Redo-Verlauf ungültig.
            Console.WriteLine($"  Ausführen:      {command.Name}");
        }

        public void Undo()
        {
            if (!_undo.TryPop(out var command))
            {
                Console.WriteLine("  Nichts zum Rückgängigmachen");
                return;
            }
            command.Undo();
            _redo.Push(command);
            Console.WriteLine($"  Rückgängig:     {command.Name}");
        }

        public void Redo()
        {
            if (!_redo.TryPop(out var command))
            {
                Console.WriteLine("  Nichts zum Wiederherstellen");
                return;
            }
            command.Execute();
            _undo.Push(command);
            Console.WriteLine($"  Wiederherstellen: {command.Name}");
        }
    }
}
