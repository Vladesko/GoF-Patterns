namespace GoFPatterns.Command.TextEditor.Commands
{
    // Command concreto - ricorda il testo eliminato per poterlo ripristinare con Undo.
    public class DeleteTextCommand(Document document, int position, int length) : ICommand
    {
        private string _deletedText = string.Empty;

        public string Name => $"Elimina {length} caratteri";

        public void Execute()
        {
            _deletedText = document.GetText(position, length);
            document.Delete(position, length);
        }

        public void Undo() => document.Insert(position, _deletedText);
    }
}
