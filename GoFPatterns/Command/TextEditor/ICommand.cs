namespace GoFPatterns.Command.TextEditor
{
    // Команда - превращает запрос в объект. Undo позволяет отменить операцию.
    public interface ICommand
    {
        string Name { get; }
        void Execute();
        void Undo();
    }
}
