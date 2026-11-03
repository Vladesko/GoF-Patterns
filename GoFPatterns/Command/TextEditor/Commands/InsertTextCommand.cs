namespace GoFPatterns.Command.TextEditor.Commands
{
    // Comando concreto: guarda el receptor y todos los parámetros necesarios para ejecutar y deshacer la operación.
    public class InsertTextCommand(Document document, int position, string text) : ICommand
    {
        public string Name => $"Insertar \"{text}\"";

        public void Execute() => document.Insert(position, text);

        public void Undo() => document.Delete(position, text.Length);
    }
}
