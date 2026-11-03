namespace GoFPatterns.Command.TextEditor
{
    // Befehl – macht aus einer Anfrage ein Objekt. Undo ermöglicht es, die Operation rückgängig zu machen.
    public interface ICommand
    {
        string Name { get; }
        void Execute();
        void Undo();
    }
}
