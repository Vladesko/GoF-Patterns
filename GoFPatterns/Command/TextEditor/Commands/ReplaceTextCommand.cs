namespace GoFPatterns.Command.TextEditor.Commands
{
    // Comando concreto construido a partir de otros comandos: reemplazar = eliminar + insertar.
    public class ReplaceTextCommand(Document document, int position, int length, string newText) : ICommand
    {
        private readonly DeleteTextCommand _delete = new(document, position, length);
        private readonly InsertTextCommand _insert = new(document, position, newText);

        public string Name => $"Reemplazar por \"{newText}\"";

        public void Execute()
        {
            _delete.Execute();
            _insert.Execute();
        }

        // Deshacer en orden inverso.
        public void Undo()
        {
            _insert.Undo();
            _delete.Undo();
        }
    }
}
