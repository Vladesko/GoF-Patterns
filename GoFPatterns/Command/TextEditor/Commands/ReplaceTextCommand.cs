namespace GoFPatterns.Command.TextEditor.Commands
{
    // Konkreter Befehl, der aus anderen Befehlen besteht – Ersetzen = Löschen + Einfügen.
    public class ReplaceTextCommand(Document document, int position, int length, string newText) : ICommand
    {
        private readonly DeleteTextCommand _delete = new(document, position, length);
        private readonly InsertTextCommand _insert = new(document, position, newText);

        public string Name => $"Durch \"{newText}\" ersetzen";

        public void Execute()
        {
            _delete.Execute();
            _insert.Execute();
        }

        // Rückgängigmachen in umgekehrter Reihenfolge.
        public void Undo()
        {
            _insert.Undo();
            _delete.Undo();
        }
    }
}
