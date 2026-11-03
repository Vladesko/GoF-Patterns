using GoFPatterns.Command.TextEditor.Commands;

namespace GoFPatterns.Command.TextEditor
{
    public class DemonstrateTextEditor
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Command: Texteditor mit Undo/Redo ===\n");

            var document = new Document();
            var history = new CommandHistory();

            void Show() => Console.WriteLine($"  Text: \"{document.Text}\"\n");

            history.Execute(new InsertTextCommand(document, 0, "Hallo"));
            history.Execute(new InsertTextCommand(document, 5, " Leute"));
            Show();

            history.Execute(new ReplaceTextCommand(document, 6, 5, "Muster"));
            Show();

            history.Execute(new DeleteTextCommand(document, 0, 6));
            Show();

            history.Undo();
            Show();
            history.Undo();
            Show();
            history.Redo();
            Show();

            // Ein neuer Befehl löscht den Redo-Verlauf.
            history.Execute(new InsertTextCommand(document, document.Text.Length, "!"));
            history.Redo();
            Show();
        }
    }
}
