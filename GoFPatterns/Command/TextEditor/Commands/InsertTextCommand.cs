namespace GoFPatterns.Command.TextEditor.Commands
{
    // Concrete command - stores receiver and all parameters needed to execute and undo the operation.
    public class InsertTextCommand(Document document, int position, string text) : ICommand
    {
        public string Name => $"Insert \"{text}\"";

        public void Execute() => document.Insert(position, text);

        public void Undo() => document.Delete(position, text.Length);
    }
}
