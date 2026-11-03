namespace GoFPatterns.Command.TextEditor.Commands
{
    // Konkreter Befehl – speichert den Empfänger und alle Parameter, die zum Ausführen und Rückgängigmachen nötig sind.
    public class InsertTextCommand(Document document, int position, string text) : ICommand
    {
        public string Name => $"\"{text}\" einfügen";

        public void Execute() => document.Insert(position, text);

        public void Undo() => document.Delete(position, text.Length);
    }
}
