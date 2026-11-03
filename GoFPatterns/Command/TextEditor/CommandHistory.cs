namespace GoFPatterns.Command.TextEditor
{
    // Invocador: ejecuta comandos y guarda su historial para Undo/Redo.
    // Trabaja solo con ICommand y no sabe qué hacen los comandos.
    public class CommandHistory
    {
        private readonly Stack<ICommand> _undo = new();
        private readonly Stack<ICommand> _redo = new();

        public void Execute(ICommand command)
        {
            command.Execute();
            _undo.Push(command);
            _redo.Clear(); // Una acción nueva invalida el historial de rehacer.
            Console.WriteLine($"  Hacer:    {command.Name}");
        }

        public void Undo()
        {
            if (!_undo.TryPop(out var command))
            {
                Console.WriteLine("  No hay nada que deshacer");
                return;
            }
            command.Undo();
            _redo.Push(command);
            Console.WriteLine($"  Deshacer: {command.Name}");
        }

        public void Redo()
        {
            if (!_redo.TryPop(out var command))
            {
                Console.WriteLine("  No hay nada que rehacer");
                return;
            }
            command.Execute();
            _undo.Push(command);
            Console.WriteLine($"  Rehacer:  {command.Name}");
        }
    }
}
