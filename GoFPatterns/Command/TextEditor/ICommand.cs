namespace GoFPatterns.Command.TextEditor
{
    // Command - trasforma una richiesta in un oggetto. Undo permette di annullare l'operazione.
    public interface ICommand
    {
        string Name { get; }
        void Execute();
        void Undo();
    }
}
