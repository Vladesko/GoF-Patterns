namespace GoFPatterns.Command.TextEditor.Commands
{
    // Command concreto - memorizza il receiver e tutti i parametri necessari per eseguire e annullare l'operazione.
    public class InsertTextCommand(Document document, int position, string text) : ICommand
    {
        public string Name => $"Inserisci \"{text}\"";

        public void Execute() => document.Insert(position, text);

        public void Undo() => document.Delete(position, text.Length);
    }
}
