namespace GoFPatterns.Command.TextEditor.Commands
{
    // Конкретная команда - хранит получателя и все параметры, нужные для выполнения и отмены операции.
    public class InsertTextCommand(Document document, int position, string text) : ICommand
    {
        public string Name => $"Вставить \"{text}\"";

        public void Execute() => document.Insert(position, text);

        public void Undo() => document.Delete(position, text.Length);
    }
}
