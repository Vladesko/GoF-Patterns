namespace GoFPatterns.Command.TextEditor.Commands
{
    // Konkreter Befehl – merkt sich den gelöschten Text, um ihn bei Undo wiederherstellen zu können.
    public class DeleteTextCommand(Document document, int position, int length) : ICommand
    {
        private string _deletedText = string.Empty;

        public string Name => $"{length} Zeichen löschen";

        public void Execute()
        {
            _deletedText = document.GetText(position, length);
            document.Delete(position, length);
        }

        public void Undo() => document.Insert(position, _deletedText);
    }
}
