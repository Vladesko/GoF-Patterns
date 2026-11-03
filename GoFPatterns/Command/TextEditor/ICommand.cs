namespace GoFPatterns.Command.TextEditor
{
    // Comando: convierte una petición en un objeto. Undo permite revertir la operación.
    public interface ICommand
    {
        string Name { get; }
        void Execute();
        void Undo();
    }
}
