namespace GoFPatterns.Command.TextEditor.Commands
{
    // Comando concreto: recuerda el texto eliminado para poder restaurarlo al deshacer.
    public class DeleteTextCommand(Document document, int position, int length) : ICommand
    {
        private string _deletedText = string.Empty;

        public string Name => $"Eliminar {length} caracteres";

        public void Execute()
        {
            _deletedText = document.GetText(position, length);
            document.Delete(position, length);
        }

        public void Undo() => document.Insert(position, _deletedText);
    }
}
