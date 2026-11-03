using GoFPatterns.Command.TextEditor.Commands;

namespace GoFPatterns.Command.TextEditor
{
    public class DemonstrateTextEditor
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Command: editor di testo con Undo/Redo ===\n");

            var document = new Document();
            var history = new CommandHistory();

            void Show() => Console.WriteLine($"  Testo: \"{document.Text}\"\n");

            history.Execute(new InsertTextCommand(document, 0, "Ciao"));
            history.Execute(new InsertTextCommand(document, 4, " mondo"));
            Show();

            history.Execute(new ReplaceTextCommand(document, 5, 5, "pattern"));
            Show();

            history.Execute(new DeleteTextCommand(document, 0, 5));
            Show();

            history.Undo();
            Show();
            history.Undo();
            Show();
            history.Redo();
            Show();

            // Un nuovo comando cancella la cronologia di redo.
            history.Execute(new InsertTextCommand(document, document.Text.Length, "!"));
            history.Redo();
            Show();
        }
    }
}
