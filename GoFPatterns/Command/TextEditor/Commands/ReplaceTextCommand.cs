namespace GoFPatterns.Command.TextEditor.Commands
{
    // Concrete command built from other commands - replace = delete + insert.
    public class ReplaceTextCommand(Document document, int position, int length, string newText) : ICommand
    {
        private readonly DeleteTextCommand _delete = new(document, position, length);
        private readonly InsertTextCommand _insert = new(document, position, newText);

        public string Name => $"Replace with \"{newText}\"";

        public void Execute()
        {
            _delete.Execute();
            _insert.Execute();
        }

        // Undo in reverse order.
        public void Undo()
        {
            _insert.Undo();
            _delete.Undo();
        }
    }
}
