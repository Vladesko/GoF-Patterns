namespace GoFPatterns.Command.TextEditor.Commands
{
    // Конкретная команда - запоминает удалённый текст, чтобы восстановить его при Undo.
    public class DeleteTextCommand(Document document, int position, int length) : ICommand
    {
        private string _deletedText = string.Empty;

        public string Name => $"Удалить {length} символов";

        public void Execute()
        {
            _deletedText = document.GetText(position, length);
            document.Delete(position, length);
        }

        public void Undo() => document.Insert(position, _deletedText);
    }
}
