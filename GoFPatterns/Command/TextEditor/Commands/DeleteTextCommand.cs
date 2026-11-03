namespace GoFPatterns.Command.TextEditor.Commands
{
    // Concrete command - remembers deleted text to be able to restore it on Undo.
    public class DeleteTextCommand(Document document, int position, int length) : ICommand
    {
        private string _deletedText = string.Empty;

        public string Name => $"Delete {length} chars";

        public void Execute()
        {
            _deletedText = document.GetText(position, length);
            document.Delete(position, length);
        }

        public void Undo() => document.Insert(position, _deletedText);
    }
}
