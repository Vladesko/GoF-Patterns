namespace GoFPatterns.Command.TextEditor.Commands
{
    // Command concreto composto da altri comandi - sostituisci = elimina + inserisci.
    public class ReplaceTextCommand(Document document, int position, int length, string newText) : ICommand
    {
        private readonly DeleteTextCommand _delete = new(document, position, length);
        private readonly InsertTextCommand _insert = new(document, position, newText);

        public string Name => $"Sostituisci con \"{newText}\"";

        public void Execute()
        {
            _delete.Execute();
            _insert.Execute();
        }

        // Undo in ordine inverso.
        public void Undo()
        {
            _insert.Undo();
            _delete.Undo();
        }
    }
}
