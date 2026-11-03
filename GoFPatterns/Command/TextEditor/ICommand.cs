namespace GoFPatterns.Command.TextEditor
{
    // Command - turns a request into an object. Undo makes it possible to revert the operation.
    public interface ICommand
    {
        string Name { get; }
        void Execute();
        void Undo();
    }
}
